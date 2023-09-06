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

        if (spawners != null)
        {
            foreach (Spawner spawner in spawners)
            {
                if (spawner != null)
                {
                    if (spawner.activationWave <= waveNumber)
                    {
                        spawner.SpawnWave();
                    }
                }
                else
                {
                    Debug.LogWarning("Null spawner in the array.");
                }
            }
        }
        else
        {
            Debug.LogWarning("Spawners array is null.");
        }

        isWaveInProgress = false;
    }
}