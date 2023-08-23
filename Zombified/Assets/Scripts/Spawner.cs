using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public GameObject Zombie3;
    public Transform spawnPoint;
    public int startingZombies;
    public int minAdditionalZombies;
    public int maxAdditionalZombies;
    public int activationWave = 1; // The wave number at which this spawner is activated
    [Range(1, 5)][SerializeField] private float enemyHPMultiplier = 2.0f;
    [Range(1, 5)][SerializeField] private float enemyDamageMultiplier = 1.5f;
    private WaveManager waveManager;


    private void Start()
    {
        waveManager = GetComponentInParent<WaveManager>();
    }

    public void SpawnWave()
    {
        int numZombies = startingZombies;

        if (waveManager.waveNumber > 1)
        {
            numZombies += waveManager.waveNumber * Random.Range(minAdditionalZombies, maxAdditionalZombies + 1);
        }

        waveManager.enemiesRemaining += numZombies;
        gameManager.instance.waveNumberText.text = "Wave " +(waveManager.waveNumber);

        for (int i = 0; i < numZombies; i++)
        {
            SpawnZombie();
        }
    }

    private void SpawnZombie()
    {
        GameObject newZombie = Instantiate(Zombie3, spawnPoint.position, spawnPoint.rotation);
        enemyAI zombieAI = newZombie.GetComponent<enemyAI>();

        int damageMultiplier = Mathf.RoundToInt(Mathf.Pow(enemyDamageMultiplier, waveManager.waveNumber - 1));
        int hpMultiplier = Mathf.RoundToInt(Mathf.Pow(enemyHPMultiplier, waveManager.waveNumber - 1));

        zombieAI.damage = damageMultiplier;
        zombieAI.HP = hpMultiplier;
        zombieAI.spawnPoint = spawnPoint;
    }

    public void SetWaveManager(WaveManager waveManager)
    {
        this.waveManager = waveManager;
    }
}
