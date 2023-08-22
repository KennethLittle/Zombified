using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;
    public Spawner[] spawners;
    public doorController doorController;
    public int waveNumber = 0;
    public int enemiesRemaining = 0;

    private bool isWaveInProgress = false;

    private void Start()
    {
        instance = this;
        StartNextWave();
    }

    private void Update()
    {
        if (enemiesRemaining == 0 && !isWaveInProgress)
        {
            StartNextWave(); // Start next wave if no enemies remain and no wave is currently spawning
        }
    }

    private void StartNextWave()
    {
        isWaveInProgress = true;
        Invoke(nameof(StartWave), 5f); // 5-second buffer before starting the next wave
    }

    private void StartWave()
    {
        if (waveNumber == 5)
        {
            doorController.OpenDoor();
        }

        waveNumber++;

        foreach (Spawner spawner in spawners)
        {
            if (spawner.activationWave <= waveNumber)
            {
                spawner.SpawnWave();
            }
        }

        isWaveInProgress = false; // Allow the next wave to start once this wave has begun spawning
    }
}