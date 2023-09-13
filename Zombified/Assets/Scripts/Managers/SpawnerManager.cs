using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    public static SpawnerManager instance;

    public Spawner[] spawners;  // Reference to all the spawners.
    public int maxEnemiesOnField = 50;  // The total number of enemies you want in the field.
    public int enemiesToSpawnAtOnce = 10;  // Number of enemies to spawn when the threshold is reached.
    public int spawnThreshold = 10;  // Start spawning when there are this many or fewer enemies remaining.

    private int currentEnemiesOnField = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        EnemyDefeated();
    }

    public void NotifyEnemyDeath()
    {
        currentEnemiesOnField--;
    }

    private void SpawnEnemies()
    {
        int enemiesToSpawn = Mathf.Min(enemiesToSpawnAtOnce, maxEnemiesOnField - currentEnemiesOnField);

        foreach (Spawner spawner in spawners)
        {
            for (int i = 0; i < enemiesToSpawn; i++)
            {
                spawner.SpawnEnemy();
                currentEnemiesOnField++;
            }
        }
    }

    public void EnemyDefeated()
    {
        if (currentEnemiesOnField <= spawnThreshold)
        {
            SpawnEnemies();
        }
    }
}
