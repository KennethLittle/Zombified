using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Renamed from Zombie3
    public Transform[] spawnPoints;
    public float timeBetweenEnemySpawns;
    public int enemiesToSpawn; // Direct number of enemies to spawn, no wave multiplication

    [Range(1, 5)][SerializeField] private float enemyHPMultiplier = 2.0f;
    [Range(1, 5)][SerializeField] private float enemyDamageMultiplier = 1.5f;

    // No need for WaveManager references since we are not using waves

    // Directly spawn enemies when called by the SpawnerManager
    public void SpawnEnemies(int count)
    {
        StartCoroutine(SpawnEnemiesCoroutine(count));
    }

    private IEnumerator SpawnEnemiesCoroutine(int count)
    {
        for (int i = 0; i < count; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(timeBetweenEnemySpawns);
        }
    }

    public void SpawnEnemy()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)]; // Choose a random spawn point
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

        enemyAI enemyAIComponent = newEnemy.GetComponent<enemyAI>();

        // If you want to retain some scaling based on the number of enemies spawned previously, 
        // you can do so, but this example uses the base stats multiplied by the set multipliers
        enemyAIComponent.damage = Mathf.RoundToInt(enemyAIComponent.damage * enemyDamageMultiplier);
        enemyAIComponent.HP = Mathf.RoundToInt(enemyAIComponent.HP * enemyHPMultiplier);
        enemyAIComponent.spawnPoint = spawnPoint; // Make sure enemyAI script has this property if it's necessary
    }
}
