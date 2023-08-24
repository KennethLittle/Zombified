using UnityEngine;
using System.Collections.Generic; 

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;
    public Spawner[] spawners;
    public List<doorController> doorControllers; 
    public int waveNumber = 0;
    public int enemiesRemaining = 0;

    private bool isWaveInProgress = false;

    private void Start()
    {
        instance = this;
    }

    private void Update()
    {
        if (enemiesRemaining == 0 && !isWaveInProgress)
        {
            StartNextWave(); 
        }
    }

    private void StartNextWave()
    {
        isWaveInProgress = true;
        Invoke(nameof(StartWave), 5f); 
    }

    private void StartWave()
    {
        waveNumber++;

        if (waveNumber == 5)
        {
            foreach (var doorController in doorControllers) 
            {
                doorController.OpenDoor();
            }
        }

        foreach (Spawner spawner in spawners)
        {
            if (spawner.activationWave <= waveNumber)
            {
                spawner.SpawnWave();
            }
        }

        isWaveInProgress = false; 
    }
}