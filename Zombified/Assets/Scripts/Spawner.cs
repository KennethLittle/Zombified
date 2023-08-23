using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    public GameObject Zombie3;
    public Transform[] spawnPoints; // Array of spawn positions
    public float timeBetweenZombieSpawns;
    public int startingZombies;
    public int minAdditionalZombies;
    public int maxAdditionalZombies;
    public int activationWave;
    [Range(1, 5)][SerializeField] private float enemyHPMultiplier = 2.0f;
    [Range(1, 5)][SerializeField] private float enemyDamageMultiplier = 1.5f;
    private WaveManager waveManager;

    private void Start()
    {
        waveManager = WaveManager.instance;
    }

    public void SpawnWave()
    {
        if (waveManager.waveNumber < activationWave || spawnPoints.Length == 0) return;

        int numZombies = startingZombies + waveManager.waveNumber * Random.Range(minAdditionalZombies, maxAdditionalZombies + 1);
        waveManager.enemiesRemaining += numZombies;
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
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)]; // Choose a random spawn point
        GameObject newZombie = Instantiate(Zombie3, spawnPoint.position, spawnPoint.rotation);
        enemyAI zombieAI = newZombie.GetComponent<enemyAI>();

        int baseDamage = zombieAI.damage;
        int baseHP = zombieAI.HP;

        int damageMultiplier = Mathf.RoundToInt(Mathf.Pow(enemyDamageMultiplier, waveManager.waveNumber - 1));
        int hpMultiplier = Mathf.RoundToInt(Mathf.Pow(enemyHPMultiplier, waveManager.waveNumber - 1));

        zombieAI.damage = baseDamage * damageMultiplier;
        zombieAI.HP = baseHP * hpMultiplier;
        zombieAI.spawnPoint = spawnPoint;
    }
}
