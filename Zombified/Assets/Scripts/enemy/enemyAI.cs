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
    private bool zedIsGroaning;
    private bool isDead = false;
    private bool pause = false;
    private bool destinationChosen = false;
    Vector3 playerDir;

    bool playerInRange;
    float angleToPlayer;
    Vector3 startingPos;
    
    float stoppingDistOrig;


    public static event Action<enemyAI> OnEnemyDeathEvent;

    void Start()
    {
        // Assuming you have a singleton or reference to your EnemyManager
        EnemyManager.Instance.RegisterEnemy(this);
        agent.stoppingDistance = meleeRange;
        enemyID = nextID++;

        // TODO: Update the player's level in enemyStats. You need a way to access player's level.
        // enemyStats.UpdatePlayerLevel(PlayerManager.instance.playerLevel); 
    }

    void Update()
    {
        float agentVel = agent.velocity.normalized.magnitude;
        anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), agentVel, Time.deltaTime * animChangeSpeed));

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

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            facePlayer();
            if (!isAttacking && angleToPlayer <= attackAngle)
            {
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
    }

    void zedGroansSFX()
    {
        if (!zedIsGroaning && enemyStats.CurrentHP > 0)
        {
            StartCoroutine(playZedGroans());
        }
    }

    IEnumerator playZedGroans()
    {
        zedIsGroaning = true;
        audioSFX.PlayOneShot(audioVoice[UnityEngine.Random.Range(0, audioVoice.Length)], audioVoiceVol);
        yield return new WaitForSeconds(UnityEngine.Random.Range(voiceTimerMin, voiceTimerMax));
        zedIsGroaning = false;
    }

    IEnumerator attack()
    {
        if (!isDead)
        {
            isAttacking = true;
            anim.SetBool("isAttacking", isAttacking);
            MeleeDamage(enemyStats.CurrentDamage); // Using stats from EnemyStat
            yield return new WaitForSeconds(attackRate);
            isAttacking = false;
            anim.SetBool("isAttacking", isAttacking);
        }
    }

    public void takeDamage(int amount)
    {
        enemyStats.CurrentHP -= amount; // Using stats from EnemyStat
        if (enemyStats.CurrentHP <= 0)
        {
            int xpReward = enemyStats.CalculateExperienceReward();
            isDead = true;
            pause = true;

            OnEnemyDeathEvent?.Invoke(this);

            // Instead of directly modifying the gameManager, let the enemy manager handle it.
            EnemyManager.Instance.HandleEnemyDeath(this);

            anim.SetBool("isDead", isDead);
            Destroy(gameObject);
        }
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
        EnemyManager.Instance.DeRegisterEnemy(this);
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

}




