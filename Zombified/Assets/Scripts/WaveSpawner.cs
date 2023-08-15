using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public GameObject Zombie3;
    public Transform spawnPoint;

    public float timeBetweenWaves;
    private float nextWaveTime;
    public float timeBetweenZombieSpawns;

    public int waveNumber;
    public int enemiesRemaining = 0;

    [Range(1, 5)][SerializeField] float enemyHPMultiplier;
    [Range(1, 5)][SerializeField] float enemyDamageMultiplier;

    public int startingZombies;
    public int minAdditionalZombies;
    public int maxAdditionalZombies;

    private bool isPaused = false;
    private bool isSpawning = false;

    void Start()
    {
        nextWaveTime = Time.time + timeBetweenWaves;
    }


    void Update()
    {
        if (!isPaused && !isSpawning && enemiesRemaining <= 0 && Time.time >= nextWaveTime)
        {
            StartCoroutine(SpawnWave());
            nextWaveTime = Time.time + timeBetweenWaves;
        }
    }

    IEnumerator SpawnWave()
    {
        isSpawning = true;

        int numZombies = startingZombies + waveNumber * Random.Range(minAdditionalZombies, maxAdditionalZombies + 1);
        enemiesRemaining = numZombies;

        waveNumber++; // Increment the wave number before spawning zombies
        gameManager.instance.waveNumberText.text = "Wave " + waveNumber;

        for (int i = 0; i < numZombies; i++)
        {
            SpawnZombie();
            yield return new WaitForSeconds(timeBetweenZombieSpawns);
        }

        if (waveNumber > 0 && waveNumber % 5 == 0 && enemiesRemaining <= 0)
        {
            gameManager.instance.updateGameGoal(-numZombies);
            gameManager.instance.escape();
        }

        isSpawning = false;
    }

    void SpawnZombie()
    {
        GameObject newZombie = Instantiate(Zombie3, spawnPoint.position, spawnPoint.rotation);
        enemyAI zombieAI = newZombie.GetComponent<enemyAI>();

        zombieAI.damage = Mathf.RoundToInt(Mathf.Pow(enemyDamageMultiplier, waveNumber - 1));
        zombieAI.HP = Mathf.RoundToInt(Mathf.Pow(enemyHPMultiplier, waveNumber - 1));
        zombieAI.spawnPoint = spawnPoint;
    }
}
