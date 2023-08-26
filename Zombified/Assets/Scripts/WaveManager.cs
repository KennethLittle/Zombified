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
        Debug.Log("StartWave invoked"); // To confirm StartWave is being called

        waveNumber++;

        if (waveNumber == 5)
        {
            if (doorControllers == null)
            {
                Debug.LogError("doorControllers list is null");
                return;
            }

            foreach (var doorController in doorControllers)
            {
                if (doorController == null)
                {
                    Debug.LogError("One of the doorController instances is null");
                    continue;
                }
                doorController.OpenDoor();
            }
        }

        if (spawners == null)
        {
            Debug.LogError("spawners array is null");
            return;
        }

        foreach (Spawner spawner in spawners)
        {
            if (spawner == null)
            {
                Debug.LogError("One of the spawner instances is null");
                continue;
            }

            if (spawner.activationWave <= waveNumber)
            {
                spawner.SpawnWave();
            }
        }

        isWaveInProgress = false;
    }
}