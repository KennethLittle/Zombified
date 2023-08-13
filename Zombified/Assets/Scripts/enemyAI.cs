using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;
    [SerializeField] Transform headPos;

    [Header("----- Enemy Physics -----")]
    [Range(1, 50)] [SerializeField] public int HP;
    [Range(1, 50)] [SerializeField] int speed;
    [Range(1, 50)] [SerializeField] int playerFaceSpeed;
    [Range(45, 360)] [SerializeField] int viewAngle;
    [Range(1, 150)] [SerializeField] int roamDist;
    [Range(0, 5)] [SerializeField] int roamTimer;
    [SerializeField] int animChangeSpeed;

    [Header("----- Attack Stats -----")]
    [Range(1, 50)] [SerializeField] public int damage;
    [Range(1, 50)] [SerializeField] float meleeRange;
    [Range(1, 50)] [SerializeField] float attackRate;
    [Range(0, 90)] [SerializeField] int strikeAngle;
    [SerializeField] GameObject meleeAttack;
    [SerializeField] Transform attackPos;

    [HideInInspector] public Transform spawnPoint;

    Vector3 playerDir;
    Vector3 startingPos;

    bool isAttacking;
    bool playerInRange;
    bool destinationChosen;

    float angleToPlayer;
    float stoppingDistOrig;

    void Start()
    {
        gameManager.instance.updateGameGoal(1);
        startingPos = transform.position;
        stoppingDistOrig = agent.stoppingDistance;
    }

    void Update()
    {
        float agentVel = agent.velocity.normalized.magnitude;
        anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), agentVel, Time.deltaTime * animChangeSpeed));

        if(playerInRange && !canSeePlayer())
        {
            StartCoroutine(roam());
        }
        else if(agent.destination != gameManager.instance.player.transform.position)
        {
            StartCoroutine(roam());
        }
    }

    bool canSeePlayer()
    {
        agent.stoppingDistance = stoppingDistOrig;
        playerDir = gameManager.instance.player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(new Vector3(playerDir.x, 0, playerDir.z), transform.forward);

        Debug.Log(angleToPlayer);
        Debug.DrawRay(headPos.position, playerDir);

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

                if (!isAttacking && angleToPlayer <= strikeAngle)
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
}
