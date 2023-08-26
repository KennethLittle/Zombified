using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    public GameObject Zombie3;
    public Transform[] spawnPoints; 
    public float timeBetweenZombieSpawns;
    public int startingZombies;
    public int minAdditionalZombies;
    public int maxAdditionalZombies;
    public int activationWave;
    [Range(1, 5)][SerializeField] private float enemyHPMultiplier = 2.0f;
    [Range(1, 5)][SerializeField] private float enemyDamageMultiplier = 1.5f;
    private WaveManager waveManager;

   
    public void SpawnWave()
    {
        if (waveManager == null)
        {
            waveManager = gameManager.instance.waveManager;
            if (waveManager == null)
            {
                Debug.LogError("waveManager instance is still null");
                return;
            }
        }

        if (waveManager.waveNumber < activationWave) return;

        int numZombies = startingZombies + waveManager.waveNumber * Random.Range(minAdditionalZombies, maxAdditionalZombies + 1);
        waveManager.enemiesRemaining += numZombies;

        if (gameManager.instance == null)
        {
            Debug.LogError("gameManager instance is null");
            return;
        }

        gameManager.instance.waveNumberText.text = "Wave " + waveManager.waveNumber;

        StartCoroutine(SpawnZombies(numZombies));
    }

    private IEnumerator SpawnZombies(int numZombies)
    {
        for (int i = 0; i < numZombies; i++)
        {
            SpawnZombie();
            yield return new WaitForSeconds(timeBetweenZombieSpawns);
        }
    }

    private void SpawnZombie()
    {
        Debug.Log("SpawnZombie invoked"); // To confirm SpawnZombie is being called

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("spawnPoints array is null or empty");
            return;
        }

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)]; // Choose a random spawn point
        GameObject newZombie = Instantiate(Zombie3, spawnPoint.position, spawnPoint.rotation);
        if (newZombie == null)
        {
            Debug.LogError("newZombie instance is null");
            return;
        }

        enemyAI zombieAI = newZombie.GetComponent<enemyAI>();
        if (zombieAI == null)
        {
            Debug.LogError("Failed to get enemyAI component from newZombie");
            return;
        }

        int baseDamage = zombieAI.damage;
        int baseHP = zombieAI.HP;

        int damageMultiplier = Mathf.RoundToInt(Mathf.Pow(enemyDamageMultiplier, waveManager.waveNumber - 1));
        int hpMultiplier = Mathf.RoundToInt(Mathf.Pow(enemyHPMultiplier, waveManager.waveNumber - 1));

        zombieAI.damage = baseDamage * damageMultiplier;
        zombieAI.HP = baseHP * hpMultiplier;
        zombieAI.spawnPoint = spawnPoint;
    }
}
