using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] public static int nextID = 0;
    [SerializeField] public int enemyID;
    [SerializeField] private Renderer model;
    [SerializeField] public NavMeshAgent agent;
    [SerializeField] Transform headPos;
    [SerializeField] private Animator anim;

    [Header("----- Stats -----")]
    public EnemyStat enemyStats; // Introducing the EnemyStat reference
    [SerializeField] private int playerFaceSpeed;
    [Range(60, 180)][SerializeField] int viewAngle;
    [Range(30, 90)] [SerializeField] int attackAngle;
    [SerializeField] private float meleeRange;
    [SerializeField] private int animChangeSpeed;
    [SerializeField] private int roamTimer;
   
    public int baseXP = 10; // XP might also be something you'd want to add in EnemyStat in future.

    [Header("----- Attack Stats -----")]
    [Range(1, 50)][SerializeField] private float attackRate;

    [Header("----- Roaming -----")]
    public float roamRadius = 10f;
    private Vector3 roamPosition;

    [Header("----- Detection -----")]
    public float detectionRange = 15f;

    [Header("----- Audio -----")]
    [SerializeField] private AudioSource audioSFX;
    [SerializeField] private AudioClip[] audioVoice;
    [SerializeField][Range(0, 1)] private float audioVoiceVol;
    [SerializeField][Range(5, 24)] private float voiceTimerMin;
    [SerializeField][Range(25, 60)] private float voiceTimerMax;

    private bool isAttacking = false;
    private bool robotIsGroaning;
    private bool isDead = false;
    private bool pause = false;
    private bool destinationChosen = false;

    private bool groundedEnemy;
    private bool footstepsIsPlaying;
    private Vector3 enemyVelocity;
    private Vector3 move;
    private int jumpCount;
    private bool isSprinting;
    private float walkVolume;
    private float audioLHVolOrig;

    Vector3 playerDir;

    bool playerInRange;
    float angleToPlayer;
    Vector3 startingPos;
    
    float stoppingDistOrig;


    public static event Action<enemyAI> OnEnemyDeathEvent;

    void Start()
    {
        // Assuming you have a singleton or reference to your EnemyManager
       // EnemyManager.Instance.RegisterEnemy(this);
        agent.stoppingDistance = meleeRange;
        enemyID = nextID++;
        foreach (var sound in AudioManager.instance.enemySFXSounds)
        {
            if (sound.name == "FootStep")
            {
                walkVolume = sound.volume;
            }
        }
        audioLHVolOrig = walkVolume;
        // TODO: Update the player's level in enemyStats. You need a way to access player's level.
        // enemyStats.UpdatePlayerLevel(PlayerManager.instance.playerLevel); 
    }

    void Update()
    {
        float agentVel = agent.velocity.normalized.magnitude;
        //anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), agentVel, Time.deltaTime * animChangeSpeed));

        if (canSeePlayer())
        {
            ChaseAndAttackPlayer();
        }
        else if (agent.remainingDistance < 0.05f)
        {
            StartCoroutine(roam());
        }
    }

    void ChaseAndAttackPlayer()
    {
        agent.stoppingDistance = meleeRange;
        agent.SetDestination(PlayerManager.instance.player.transform.position);
        anim.SetBool("isRoaming", false);
        anim.SetBool("isChasing", true);

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            facePlayer();
            if (!isAttacking && angleToPlayer <= attackAngle)
            {
                anim.SetBool("isChasing", false);
                StartCoroutine(attack());
            }
        }
    }

    bool canSeePlayer()
    {
        agent.stoppingDistance = stoppingDistOrig;
        playerDir = PlayerManager.instance.player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(new Vector3(playerDir.x, 0, playerDir.z), transform.forward);

        RaycastHit hit;

        if (Physics.Raycast(headPos.position, playerDir, out hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer <= viewAngle)
            {
                agent.SetDestination(PlayerManager.instance.player.transform.position);

                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    facePlayer();
                }

                if (isAttacking && angleToPlayer <= attackAngle)
                {
                    StartCoroutine(attack());
                }

                return true;
            }
        }
        agent.stoppingDistance = 0;
        return false;
    }

    IEnumerator roam()
    {
        if (agent.remainingDistance < 0.05f && !destinationChosen)
        {
            anim.SetBool("isRoaming", true);
            destinationChosen = true;
            agent.stoppingDistance = 0;
            yield return new WaitForSeconds(roamTimer);

            Vector3 randomPos = UnityEngine.Random.insideUnitSphere * roamRadius;
            randomPos += startingPos;

            NavMeshHit hit;
            NavMesh.SamplePosition(randomPos, out hit, roamRadius, 1);
            agent.SetDestination(hit.position);

            destinationChosen = false;
        }
    }

    void facePlayer()
    {
        Quaternion rot = Quaternion.LookRotation(playerDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * playerFaceSpeed);
        zedGroansSFX();
    }

    void zedGroansSFX()
    {
        if (!robotIsGroaning && enemyStats.CurrentHP > 0)
        {
            StartCoroutine(playRobotGroans());
        }
    }

    IEnumerator playRobotGroans()
    {
        robotIsGroaning = true;

        AudioManager.instance.PlaySound("RobotGroan", AudioManager.instance.enemySFXSounds);

        yield return new WaitForSeconds(UnityEngine.Random.Range(10f, 30f));

        while (true)
        {
            AudioManager.instance.PlaySound("RobotGroan", AudioManager.instance.enemySFXSounds);
            yield return new WaitForSeconds(UnityEngine.Random.Range(10f, 30f));

        }
        //robotIsGroaning = false;
    }

    IEnumerator attack()
    {
        if (!isDead)
        {
            anim.SetBool("isAttacking", true);
            MeleeDamage(enemyStats.CurrentDamage); // Using stats from EnemyStat
            yield return new WaitForSeconds(attackRate);
            isAttacking = false;
        }
        anim.SetBool("isAttacking", false);
    }

    public void takeDamage(int amount)
    {
        StartCoroutine(flashDamage());
        enemyStats.CurrentHP -= amount; // Using stats from EnemyStat
        anim.SetTrigger("isDamaged");
        if (enemyStats.CurrentHP <= 0)
        {
            int xpReward = enemyStats.CalculateExperienceReward();
            isDead = true;
            pause = true;

            OnEnemyDeathEvent?.Invoke(this);

            // Instead of directly modifying the gameManager, let the enemy manager handle it.
            EnemyManager.Instance.HandleEnemyDeath(this);

            anim.SetTrigger("isDead");
            Destroy(gameObject);
        }
        AudioManager.instance.PlaySound("TakeDamage", AudioManager.instance.enemySFXSounds);
    }

    IEnumerator flashDamage()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = Color.white;
    }

    public void MeleeDamage(int amount)
    {
        PlayerManager.instance.playerScript.takeDamage(amount);
    }

    void OnDestroy()
    {
       // EnemyManager.Instance.DeRegisterEnemy(this);
    }

    public void SetData(EnemyData data)
    {
        
        // Use the data to set the properties of this enemy
        this.enemyID = data.enemyID;
        this.enemyStats.CurrentHP = data.currentHP;
        this.enemyStats.baseDamage= data.baseDamage;
        this.enemyStats.baseDefense= data.baseDefense;
        Vector3 enemyPosition = data.position;
        // Add more as needed
    }


    void HandleGroundedState()
    {
        if (groundedEnemy)
        {
            enemyVelocity.y = 0f; // Ensures the enemy does not accumulate downward velocity when grounded.
            jumpCount = 0;

            if (!footstepsIsPlaying && move.normalized.magnitude > 0.5f && enemyStats.currentHP > 0)
            {
                StartCoroutine(playFootsteps());
            }
        }
    }

    IEnumerator playFootsteps()
    {
        footstepsIsPlaying = true;
        // Plays footsteps audio sfx - Plays a random footsteps sfx from the range audioFootsteps at a volume defined by audioFootstepsVol

        AudioManager.instance.PlaySound("FootStep", AudioManager.instance.enemySFXSounds);

        //// this code is for when we add a run feature to the enemy
        //if (!isSprinting)
        //{
        //    foreach (var sound in AudioManager.instance.PlayerSounds)
        //    {
        //        if (sound.name == "Footsteps")
        //        {
        //            yield return new WaitForSeconds(sound.rate);
        //        }
        //    }
        //}
        //else
        //{
        foreach (var sound in AudioManager.instance.PlayerSounds)
        {
            if (sound.name == "Footsteps")
            {
                yield return new WaitForSeconds(sound.rate / 2);
            }
        }
        //}
        footstepsIsPlaying = false;
    }

}




