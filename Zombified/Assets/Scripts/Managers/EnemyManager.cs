using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    // Static instance of the EnemyManager which allows it to be accessed by any other script.
    public static EnemyManager Instance { get; private set; }
    private GameObject enemyPrefab;

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
        SpawnerManager.instance.EnemyDefeated();
        QuestManager.instance.NotifyEnemyKilled(enemyPrefab);
        
    }

    public List<EnemyData> GetAllEnemyData()
    {
        List<EnemyData> allData = new List<EnemyData>();

        foreach (enemyAI enemy in activeEnemies)
        {
            EnemyStat enemyStat = enemy.GetComponent<EnemyStat>();
            if (enemyStat != null)
            {
                allData.Add(enemyStat.ExtractData());
            }
        }

        return allData;
    }

    public void LoadEnemyData(List<EnemyData> enemyDataList)
    {
        // Logic to set the game state based on the list of EnemyData.
        // This could involve instantiating enemies at saved positions, setting their HP, etc.

        foreach (EnemyData data in enemyDataList)
        {
            // Example: Instantiate enemy prefab at the saved position.
            GameObject enemyInstance = Instantiate(enemyPrefab, data.position, Quaternion.identity);

            enemyAI enemyScript = enemyInstance.GetComponent<enemyAI>();
            if (enemyScript != null)
            {
                // Assuming enemyAI or EnemyStat script has a method to set data
                enemyScript.SetData(data);
            }
        }
    }
}
