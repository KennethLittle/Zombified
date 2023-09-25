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
    public GameObject source;
  

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

    private GameObject enemy;

    Vector3 playerDir;

    bool playerInRange;
    float angleToPlayer;
    Vector3 startingPos;

    private GameObject _player;
    private GameObject Player
    {
        get
        {
            if (_player == null && PlayerManager.instance != null)
            {
                _player = PlayerManager.instance.player;
            }
            return _player;
        }
    }

    float stoppingDistOrig;


    public static event Action<enemyAI> OnEnemyDeathEvent;

    void Start()
    {
        // Assuming you have a singleton or reference to your EnemyManager
       EnemyManager.Instance.RegisterEnemy(this);
        agent.stoppingDistance = meleeRange;
        enemyID = nextID++;
        startingPos = transform.position;
        //foreach (var sound in AudioManager.instance.enemySFXSounds)
        //{
        //    if (sound.name == "FootStep")
        //    {
        //        walkVolume = sound.volume;
        //    }
        //}
        audioLHVolOrig = walkVolume;
    }

    void Update()
    {
       // float agentVel = agent.velocity.normalized.magnitude;
       
        if (canSeePlayer())
        {
            ChaseAndAttackPlayer();
        }
        if (agent.remainingDistance < 0.05f)
        {
            StartCoroutine(roam());
        }
    }

    void ChaseAndAttackPlayer()
    {
        agent.stoppingDistance = meleeRange;
        agent.SetDestination(Player.transform.position);

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
        playerDir = Player.transform.position - headPos.position;
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

       // AudioManager.instance.PlaySound("RobotGroan", AudioManager.instance.enemySFXSounds);

        yield return new WaitForSeconds(UnityEngine.Random.Range(10f, 30f));

        while (true)
        {
           // AudioManager.instance.PlaySound("RobotGroan", AudioManager.instance.enemySFXSounds);
            yield return new WaitForSeconds(UnityEngine.Random.Range(10f, 30f));

        }
        //robotIsGroaning = false;
    }

    IEnumerator attack()
    {
        if (!isDead)
        {
            MeleeDamage(enemyStats.CurrentDamage); // Using stats from EnemyStat
            yield return new WaitForSeconds(attackRate);
            isAttacking = false;
        }
    }

    public void takeDamage(int amount, GameObject source)
    {
        StartCoroutine(flashDamage());
        enemyStats.CurrentHP -= amount;
        anim.SetTrigger("isDamaged");

        if (enemyStats.CurrentHP <= 0)
        {
            anim.SetTrigger("isDead");
            enabled = false;
            int xpReward = enemyStats.CalculateExperienceReward();
            isDead = true;
            OnEnemyDeathEvent?.Invoke(this);
            Destroy(gameObject,3);

            
        }
        //AudioManager.instance.PlaySound("TakeDamage", AudioManager.instance.enemySFXSounds);
    }

    IEnumerator flashDamage()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = Color.white;
    }

    public void MeleeDamage(int amount)
    {
        if (Player != null)
        {
            anim.SetTrigger("isAttacking");
            Player.GetComponent<playerController>().takeDamage(amount, source);
        }
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
                //StartCoroutine(playFootsteps());
            }
        }
    }

}




