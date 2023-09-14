using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] private Renderer model;
    [SerializeField] public NavMeshAgent agent;
    [SerializeField] private Animator anim;

    [Header("----- Stats -----")]
    public EnemyStat enemyStats; // Introducing the EnemyStat reference
    [SerializeField] private int playerFaceSpeed;
    [SerializeField] private float meleeRange;
    [SerializeField] private int animChangeSpeed;
    public int ID { get; set; }
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

    private float lastPathCheckTime;
    [SerializeField] private float recheckPathInterval = 3f;
    public static event Action<enemyAI> OnEnemyDeathEvent;

    void Start()
    {
        // Assuming you have a singleton or reference to your EnemyManager
        EnemyManager.Instance.RegisterEnemy(this);
        InitiateRoaming();
        agent.stoppingDistance = meleeRange;
        ID = 0;

        // TODO: Update the player's level in enemyStats. You need a way to access player's level.
        // enemyStats.UpdatePlayerLevel(PlayerManager.instance.playerLevel); 
    }

    void Update()
    {
        if (!isAttacking && !pause)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, PlayerManager.instance.player.transform.position);

            if (distanceToPlayer <= detectionRange)
            {
                // Player detected, chase the player
                agent.SetDestination(PlayerManager.instance.player.transform.position);

                if (distanceToPlayer <= meleeRange)
                {
                    zedGroansSFX();
                    StartCoroutine(attack());
                }
            }
            else
            {
                // No player detected, roam around
                if (!agent.hasPath || agent.remainingDistance < 1f)
                {
                    roamPosition = GetRandomRoamingPosition();
                    agent.SetDestination(roamPosition);
                }
            }

            float agentVel = agent.velocity.normalized.magnitude;
            anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), agentVel, Time.deltaTime * animChangeSpeed));
        }
    }

    private Vector3 GetRandomRoamingPosition()
    {
        float randomX = UnityEngine.Random.Range(-roamRadius, roamRadius);
        float randomZ = UnityEngine.Random.Range(-roamRadius, roamRadius);

        return new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
    }

    public void InitiateRoaming()
    {
        roamPosition = GetRandomRoamingPosition();
        agent.SetDestination(roamPosition);
    }

    public void InvestigatePoint(Vector3 point)
    {
        agent.SetDestination(point);
        // You might want to add additional behavior, like increasing the enemy's speed temporarily.
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

}




