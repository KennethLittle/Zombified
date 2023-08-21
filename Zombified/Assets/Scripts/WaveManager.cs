using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;
    public Spawner[] spawners; // The spawners you can assign in the Inspector
    public gameManager gameManager;
    public float defaultTimeBetweenWaves = 240.0f;
    public float countdownTime;
    private float nextWaveTime;
    private float waveCountdownTimer;
    public doorController doorController;
    public int waveNumber;
    public int enemiesRemaining = 0;
    private bool isPaused = false;
    private bool isFirstWaveSpanwed = false;
    private List<Spawner> activeSpawners = new List<Spawner>();

    private void Start()
    {
        instance = this;
        gameManager = GetComponent<gameManager>();
        nextWaveTime = Time.time;
        waveCountdownTimer = defaultTimeBetweenWaves;
        Invoke(nameof(StartFirstWave), 5f);
        ActivateSpawnersForWave();
    }

    private void Update()
    {
        if (!isPaused && enemiesRemaining <= 0 && waveCountdownTimer > 0)
        {
            waveCountdownTimer -= Time.deltaTime;

            if (waveCountdownTimer <= 0f)
            {
                waveNumber++;

                if (waveNumber % 5 == 0 && waveNumber > 1)
                {
                    doorController.OpenDoor();
                }

                // Activate spawners based on wave number
                ActivateSpawnersForWave();

                nextWaveTime = Time.time + defaultTimeBetweenWaves;
                waveCountdownTimer = defaultTimeBetweenWaves;
            }
        }
        else
        {
            waveCountdownTimer = countdownTime;
        }
    }

    private void ActivateSpawnersForWave()
    {
        activeSpawners.Clear();
        foreach (Spawner spawner in spawners)
        {
            if (spawner.activationWave <= waveNumber)
            {
                spawner.SetWaveManager(this); // Set the WaveManager for this spawner
                activeSpawners.Add(spawner);
            }
        }

        foreach (Spawner spawner in activeSpawners)
        {
            spawner.SpawnWave();
        }
    }

    private void StartFirstWave()
    {
        isFirstWaveSpanwed = true;
        ActivateSpawnersForWave();
    }

    public void ResumeWave()
    {
        foreach (Spawner spawner in spawners)
        {
            spawner.SetWaveManager(this);
            spawner.SpawnWave();
        }
        nextWaveTime = Time.time + defaultTimeBetweenWaves;
    }

}
