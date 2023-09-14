using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    // Static instance of the EnemyManager which allows it to be accessed by any other script.
    public static EnemyManager Instance { get; private set; }

    public List<enemyAI> activeEnemies; // Store all active enemies

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Register an enemy when it's spawned
    public void RegisterEnemy(enemyAI enemy)
    {
        if (!activeEnemies.Contains(enemy))
        {
            activeEnemies.Add(enemy);
        }
    }

    // De-register an enemy when it's destroyed or killed
    public void DeRegisterEnemy(enemyAI enemy)
    {
        if (activeEnemies.Contains(enemy))
            activeEnemies.Remove(enemy);
    }

    private void OnEnable()
    {
        enemyAI.OnEnemyDeathEvent += HandleEnemyDeath;
    }

    private void OnDisable()
    {
        enemyAI.OnEnemyDeathEvent -= HandleEnemyDeath;
    }

    public void HandleEnemyDeath(enemyAI enemy)
    {
        // Handle XP gain, game goals, and other logic related to enemy death.
        gameManager.instance.updateGameGoal(-1);
        SpawnerManager.instance.EnemyDefeated();
    }
}
