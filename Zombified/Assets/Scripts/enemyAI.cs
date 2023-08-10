using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour, IDamage
{
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;

    [SerializeField] int HP;
    [SerializeField] int speed;
    [SerializeField] int playerFaceSpeed;
    [SerializeField] int damage;
    [SerializeField] GameObject meleeAttack;
    [SerializeField] float meleeRange;

    [SerializeField] float attackRate;
    [SerializeField] Transform attackPos;

    Vector3 playerDir;
    bool isAttacking;

    // Start is called before the first frame update
    void Start()
    {
        gameManager.instance.updateGameGoal(1);
    }

    // Update is called once per frame
    void Update()
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
}
