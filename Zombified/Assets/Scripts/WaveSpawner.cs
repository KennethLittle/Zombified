using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public GameObject Zombie3;
    public Transform spawnPoint;
    public gameManager gameManager;

    public float defaultTimeBetweenWaves = 240.0f;  // 4 minutes in seconds
    public float countdownTime;
    private float nextWaveTime;
    private float waveCountdownTimer;
    public float timeBetweenZombieSpawns;
    public doorController doorController;

    public int waveNumber;
    public int enemiesRemaining = 0;

    [Range(1, 5)][SerializeField] private float enemyHPMultiplier = 2.0f;
    [Range(1, 5)][SerializeField] private float enemyDamageMultiplier = 1.5f;

    public int startingZombies;
    public int minAdditionalZombies;
    public int maxAdditionalZombies;

    private bool isPaused = false;
    private bool isSpawning = false;
    private bool isFirstWaveSpanwed = false;

    private void Start()
    {
        gameManager = GetComponent<gameManager>();
        nextWaveTime = Time.time;
        waveCountdownTimer = defaultTimeBetweenWaves; // Initialize the countdown timer
        Invoke(nameof(StartFirstWave), 5f);
    }

    private void Update()
    {
        if (!isPaused && !isSpawning)
        {
            if (isFirstWaveSpanwed && enemiesRemaining <= 0 && waveCountdownTimer > 0)
            {
                waveCountdownTimer -= Time.deltaTime;

                if (waveCountdownTimer <= 0f)
                {
                    if (waveNumber % 5 == 0 && waveNumber > 1)
                    {
                        doorController.OpenDoor();
                    }

                    // Start spawning the wave
                    StartCoroutine(SpawnWave());
                    nextWaveTime = Time.time + defaultTimeBetweenWaves;
                    waveCountdownTimer = defaultTimeBetweenWaves; // Reset countdown timer
                }
            }
            else
            {
                waveCountdownTimer = countdownTime; // Set countdown timer to a specified value when enemies are present
            }
        }
    }

    private IEnumerator SpawnWave()
    {
        isSpawning = true;

        int numZombies = startingZombies + waveNumber * Random.Range(minAdditionalZombies, maxAdditionalZombies + 1);
        enemiesRemaining = numZombies;

        waveNumber++;
        gameManager.instance.waveNumberText.text = "Wave " + waveNumber;

        for (int i = 0; i < numZombies; i++)
        {
            SpawnZombie();
            yield return new WaitForSeconds(timeBetweenZombieSpawns);
        }

        isSpawning = false;
    }

    private void SpawnZombie()
    {
        GameObject newZombie = Instantiate(Zombie3, spawnPoint.position, spawnPoint.rotation);
        enemyAI zombieAI = newZombie.GetComponent<enemyAI>();

        int damageMultiplier = Mathf.RoundToInt(Mathf.Pow(enemyDamageMultiplier, waveNumber - 1));
        int hpMultiplier = Mathf.RoundToInt(Mathf.Pow(enemyHPMultiplier, waveNumber - 1));

        zombieAI.damage = damageMultiplier;
        zombieAI.HP = hpMultiplier;
        zombieAI.spawnPoint = spawnPoint;
    }

    public void ResumeWave()
    {
        StartCoroutine(SpawnWave());
        nextWaveTime = Time.time + defaultTimeBetweenWaves;
    }

    private void StartFirstWave()
    {
        isFirstWaveSpanwed = true;
        StartCoroutine(SpawnWave());
    }
}
