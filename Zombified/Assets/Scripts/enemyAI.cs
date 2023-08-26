using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    [SerializeField] public NavMeshAgent agent;
    [SerializeField] Animator anim;
    [SerializeField] Transform headPos;

    [SerializeField] public int HP;
    [SerializeField] int playerFaceSpeed;
    [SerializeField] float meleeRange;
    [Range(60, 180)][SerializeField] int viewAngle;
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
    bool isMoving;
    bool isDead = false;
    bool pause = false;

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
        UpdatePlayerPosition(); // Constantly update player position

        // Calculate agent velocity and update animation
        float agentVel = agent.velocity.normalized.magnitude;
        anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), agentVel, Time.deltaTime * animChangeSpeed));

        if (!isAttacking && !pause)
        {
            float someThreshold = 0.5f;  // You can adjust this value
            if (agent.pathStatus != NavMeshPathStatus.PathComplete && agent.remainingDistance > someThreshold)
            {
                agent.ResetPath();
            }
        }

        if (!isAttacking && pause == false)
        {
            if (Vector3.Distance(transform.position, gameManager.instance.player.transform.position) <= meleeRange)
            {
                zedGroansSFX();
                StartCoroutine(attack());
            }
        }
    }

    void UpdatePlayerPosition()
    {
        RaycastHit hit;
        Vector3 directionToPlayer = (gameManager.instance.player.transform.position - transform.position).normalized;
        if (Physics.Raycast(transform.position, directionToPlayer, out hit))
        {
            if (hit.collider.CompareTag("Player"))
            {
                agent.SetDestination(gameManager.instance.player.transform.position);
            }
            else
            {
                // Obstacle detected; you can decide what action to take here.
                agent.ResetPath();
            }
        }
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


    void facePlayer()
    {
        Quaternion rot = Quaternion.LookRotation(playerDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * playerFaceSpeed);
    }

    IEnumerator attack()
    {
        if (!isDead)
        {
            isAttacking = true;
            anim.SetBool("isAttacking", isAttacking);
            MeleeDamage(damage);

            yield return new WaitForSeconds(attackRate);

            isAttacking = false;
            anim.SetBool("isAttacking", isAttacking);

        }
    }
    public void takeDamage(int amount)
    {
        HP -= amount;
        UpdatePlayerPosition();
        StartCoroutine(flashDamage());

        if (HP <= 0)
        {
            isDead = true;
            pause = true;
            gameManager.instance.updateGameGoal(-1);
            gameManager.instance.levelUpSystem.GainXP(WaveManager.instance.waveNumber * 7);
            anim.SetBool("isDead", isDead);
            enabled = false;
            Destroy(gameObject,5.0f);
            WaveManager wave = GameObject.FindObjectOfType<WaveManager>();
            if (wave != null)
            {
                wave.enemiesRemaining--;
            }
        }
    }

    IEnumerator flashDamage()
    {
        
        model.material.color = Color.red; // Set to red
        yield return new WaitForSeconds(0.1f);
        model.material.color = Color.white; // Restore the original color
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
        enabled = true;
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
        }
    }

    private void UpdateBaseXP()
    {
        baseXP += WaveManager.instance.waveNumber * 7;
    }

}
