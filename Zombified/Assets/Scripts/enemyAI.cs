using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour, IDamage
{
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;

    [SerializeField] public int HP;
    [SerializeField] int baseXP = 10;
    [SerializeField] int speed;
    [SerializeField] int playerFaceSpeed;
    [SerializeField] public int damage;
    [SerializeField] GameObject meleeAttack;
    [SerializeField] float meleeRange;

    [SerializeField] float attackRate;
    [SerializeField] Transform attackPos;

    [HideInInspector] public Transform spawnPoint;
    public static event System.Action<int> OnEnemyKilled;


    Vector3 playerDir;
    bool isAttacking;
    bool playerInRange;

    // Start is called before the first frame update
    void Start()
    {
        gameManager.instance.updateGameGoal(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange)
        {
            playerDir = gameManager.instance.player.transform.position - transform.position;

            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                facePlayer();

                if (!isAttacking)
                {
                    if (playerDir.magnitude <= meleeRange)
                    {
                        StartCoroutine(attack());
                    }
                }

            }

            agent.SetDestination(gameManager.instance.player.transform.position);
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

        dealMeleeDamage(damage);

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

    public void dealMeleeDamage(int damage)
    {
        gameManager.instance.playerScript.takeDamage(damage);
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
        }
    }

    private void UpdateBaseXP()
    {
        baseXP += gameManager.instance.waveSpawnerScript.waveNumber * 7;
    }
}
