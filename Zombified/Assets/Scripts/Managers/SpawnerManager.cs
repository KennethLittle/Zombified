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

    private void Start()
    {
        SpawnEnemies();
    }

    private void Update()
    {
         
    }

    public void NotifyEnemyDeath()
    {
        currentEnemiesOnField--;
        EnemyDefeated();  // Check if we need to spawn new enemies after each enemy death
    }

    private void SpawnEnemies()
    {
        int availableSpacesOnField = maxEnemiesOnField - currentEnemiesOnField;
        int enemiesToSpawn = Mathf.Min(enemiesToSpawnAtOnce, availableSpacesOnField);

        int baseEnemiesForSpawner = enemiesToSpawn / spawners.Length;
        int remainingEnemies = enemiesToSpawn % spawners.Length;

        foreach (Spawner spawner in spawners)
        {
            // Base number of enemies that should be spawned by this spawner
            int enemiesForThisSpawner = baseEnemiesForSpawner;

            // If there are remaining enemies, add one to this spawner and decrement the remaining count
            if (remainingEnemies > 0)
            {
                enemiesForThisSpawner++;
                remainingEnemies--;
            }

            spawner.SpawnEnemies(enemiesForThisSpawner);
            currentEnemiesOnField += enemiesForThisSpawner;
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
