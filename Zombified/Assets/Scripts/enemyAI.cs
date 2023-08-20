using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;
    [SerializeField] Transform headPos;

    [SerializeField] public int HP;
    [SerializeField] int speed;
    [SerializeField] int playerFaceSpeed;
    [SerializeField] float meleeRange;
    [Range(60, 180)][SerializeField] int viewAngle;
    [Range(1, 500)][SerializeField] int roamDist;
    [Range(0, 3)][SerializeField] int roamTimer;
    [SerializeField] int animChangeSpeed;

    public int baseXP = 10;


    [Header("----- Attack Stats -----")]
    [SerializeField] public int damage;
    [Range(1, 50)][SerializeField] float attackRate;
    [Range(0, 90)][SerializeField] int strikeAngle;
    [SerializeField] GameObject meleeAttack;
    [SerializeField] Transform attackPos;

    [HideInInspector] public Transform spawnPoint;
    
    [Header("----- Audio -----")]
    // audio<something> is an array of sfx
    // audio<something>Vol is the sfx volume
    [SerializeField] AudioSource audioSFX;
    [SerializeField] AudioClip[] audioVoice;
    [SerializeField] [Range(0, 1)] float audioVoiceVol;
    [SerializeField] [Range(5, 24)] float voiceTimerMin;
    [SerializeField] [Range(25, 60)] float voiceTimerMax;
    public static event System.Action<int> OnEnemyKilled;

    Vector3 playerDir;
    Vector3 startingPos;


    bool playerInRange;
    bool playerInAttackRange;
    bool destinationChosen;
    bool isAttacking = false;
    bool zedIsGroaning;

    float angleToPlayer;
    float stoppingDistOrig;

    void Start()
    {
        gameManager.instance.updateGameGoal(1);
        startingPos = transform.position;
        stoppingDistOrig = agent.stoppingDistance;
        meleeRange = agent.stoppingDistance;


    }

    void Update()
    {
        float agentVel = agent.velocity.normalized.magnitude;
        anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), agentVel, Time.deltaTime * animChangeSpeed));


        if (!isAttacking) // Check if not already attacking
        {
            if (canSeePlayer()) // Chase the player if they can be seen
            {
                if (Vector3.Distance(transform.position, gameManager.instance.player.transform.position) <= meleeRange)
                {
                    StartCoroutine(attack());
                }
            }
            if (playerInRange && !canSeePlayer())
            {
                StartCoroutine(roam());
            }
            else if (agent.destination != gameManager.instance.player.transform.position)
            {
                StartCoroutine(roam());
            }
        }

        zedGroansSFX();

    }

    void zedGroansSFX()
    {
        if (!zedIsGroaning && HP > 0)
        {
            StartCoroutine(playZedGroans());
        }
    }

    IEnumerator playZedGroans()
    {
        zedIsGroaning = true;
        // Plays zombie groans audio sfx - Plays a random zombie groan sfx from the range audioVoice at a volume defined by audioVoiceVol
        audioSFX.PlayOneShot(audioVoice[Random.Range(0, audioVoice.Length)], audioVoiceVol);
        if (HP > 0)
        {
            Random.seed = System.DateTime.Now.Millisecond;
            yield return new WaitForSeconds(Random.Range(voiceTimerMin, voiceTimerMax));
        }
        zedIsGroaning = false;
    }

    bool canSeePlayer()
    {
        agent.stoppingDistance = stoppingDistOrig;
        playerDir = gameManager.instance.player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(new Vector3(playerDir.x, 0, playerDir.z), transform.forward);

        RaycastHit hit;

        if (Physics.Raycast(headPos.position, playerDir, out hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer <= viewAngle)
            {
                agent.SetDestination(gameManager.instance.player.transform.position);

                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    facePlayer();
                }

                if (!isAttacking && angleToPlayer <= strikeAngle && Vector3.Distance(transform.position, gameManager.instance.player.transform.position) <= meleeRange)
                {
                    playerInAttackRange = true; // Set to true only if not attacking and player in attack range
                    
                }
                else
                {
                    playerInAttackRange = false;
                    
                }

                return true;
            }
            else if (hit.collider.CompareTag("Obstacle"))
            {
                // Choose a new destination to avoid the obstacle
                Vector3 avoidDirection = Vector3.Cross(Vector3.up, playerDir.normalized).normalized;
                NavMeshHit navHit;
                if (NavMesh.SamplePosition(transform.position + avoidDirection * 2f, out navHit, 2f, NavMesh.AllAreas))
                {
                    agent.SetDestination(navHit.position);
                }
            }
        }

        agent.stoppingDistance = 0;
        return false;
    }

    IEnumerator roam()
    {
        if (agent.remainingDistance <= 0.05f && !destinationChosen)
        {
            destinationChosen = true;
            agent.stoppingDistance = 0;
            yield return new WaitForSeconds(roamTimer);
            Vector3 randomPos = Random.insideUnitSphere * roamDist;
            randomPos += startingPos;

            NavMeshHit hit;
            NavMesh.SamplePosition(randomPos, out hit, roamDist, 1);
            agent.SetDestination(hit.position);

            destinationChosen = false;
        }
    }

    void facePlayer()
    {
        Quaternion rot = Quaternion.LookRotation(playerDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * playerFaceSpeed);
    }

    IEnumerator attack()
    {
        isAttacking = true;

        MeleeDamage(damage);

        yield return new WaitForSeconds(attackRate);

        isAttacking = false;
    }

    public void takeDamage(int amount)
    {
        HP -= amount;
        StartCoroutine(flashDamage());

        if (HP <= 0)
        {
            gameManager.instance.updateGameGoal(-1);
            gameManager.instance.levelUpSystem.GainXP(gameManager.instance.waveSpawnerScript.waveNumber * 7);
            Destroy(gameObject);

            WaveSpawner waveSpawner = GameObject.FindObjectOfType<WaveSpawner>();
            if (waveSpawner != null)
            {
                waveSpawner.enemiesRemaining--;
            }
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
        gameManager.instance.playerScript.takeDamage(amount);
    }

    private void OnDestroy()
    {
        if (OnEnemyKilled != null)
        {
            gameManager.instance.levelUpSystem.GainXP(baseXP);
            OnEnemyKilled(1);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            agent.stoppingDistance = 0;
        }
    }

    private void UpdateBaseXP()
    {
        baseXP += gameManager.instance.waveSpawnerScript.waveNumber * 7;
    }

}
