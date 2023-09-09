using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    [SerializeField] public NavMeshAgent agent;
    [SerializeField] Animator anim;

    [Header("----- Stats -----")]
    [SerializeField] public int HP;
    [SerializeField] int playerFaceSpeed;
    [SerializeField] float meleeRange;
    [SerializeField] int animChangeSpeed;
    public int ID { get; set; }

    public int baseXP = 10;

    [Header("----- Attack Stats -----")]
    [SerializeField] public int damage;
    [Range(1, 50)][SerializeField] float attackRate;

    [Header("----- Audio -----")]
    [SerializeField] AudioSource audioSFX;
    [SerializeField] AudioClip[] audioVoice;
    [SerializeField][Range(0, 1)] float audioVoiceVol;
    [SerializeField][Range(5, 24)] float voiceTimerMin;
    [SerializeField][Range(25, 60)] float voiceTimerMax;

    
    public static event System.Action<int> OnEnemyKilled;
    public Transform spawnPoint;
    [SerializeField] float recheckPathInterval = 3f; // Time in seconds the enemy waits before rechecking path
    private float lastPathCheckTime;

    public Quest quest;
    bool isAttacking = false;
    bool zedIsGroaning;
    bool isDead = false;
    bool pause = false;

    void Start()
    {
        gameManager.instance.updateGameGoal(1);
        agent.stoppingDistance = meleeRange;
        ID = 0; //enemy ID #
    }

    void Update()
    {
        if (!isAttacking && !pause)
        {
            if (Time.time >= lastPathCheckTime + recheckPathInterval)
            {
                UpdatePlayerPosition();
                lastPathCheckTime = Time.time;
            }

            float agentVel = agent.velocity.normalized.magnitude;
            anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), agentVel, Time.deltaTime * animChangeSpeed));

            if (Vector3.Distance(transform.position, gameManager.instance.player.transform.position) <= meleeRange)
            {
                zedGroansSFX();
                StartCoroutine(attack());
            }
        }
    }

    public void UpdatePlayerPosition()
    {
        agent.SetDestination(gameManager.instance.player.transform.position);
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
        audioSFX.PlayOneShot(audioVoice[Random.Range(0, audioVoice.Length)], audioVoiceVol);
        yield return new WaitForSeconds(Random.Range(voiceTimerMin, voiceTimerMax));
        zedIsGroaning = false;
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
        if (HP <= 0)
        {
            isDead = true;
            pause = true;
            gameManager.instance.updateGameGoal(-1);
            gameManager.instance.levelUpSystem.GainXP(WaveManager.instance.waveNumber * 7);
            anim.SetBool("isDead", isDead);
            Destroy(gameObject);
            WaveManager wave = GameObject.FindAnyObjectByType<WaveManager>();
            if(wave != null)
            {
                wave.enemiesRemaining--;
            }
            //if (quest.isActive)
            //{
            //    quest.goal.EnemyKilled();
            //    if (quest.goal.Complete())
            //    {
            //        // turn in quest to NPC
            //    }
            //}
           
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
            OnEnemyKilled(1);
        }
    }

    // This function is called when the enemy enters a trigger zone
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BayDoor"))
        {
            
        }
    }

    // This function is called when the enemy exits a trigger zone
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("BayDoor"))
        {
            
        }
    }

}
