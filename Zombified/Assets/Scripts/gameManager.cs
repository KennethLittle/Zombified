using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class gameManager : MonoBehaviour
{

    public static gameManager instance;
    public GameObject playerSpawnPos;

    public GameObject waveSpawner;
    public WaveSpawner waveSpawnerScript;

    public GameObject player;
    public playerController playerScript;

    public GameObject activeMenu;
    public GameObject pauseMenu;
    public GameObject loseMenu;
    public GameObject escapeMenu;
    public TextMeshProUGUI enemiesRemainingText;
    public TextMeshProUGUI waveNumberText;
    public Image playerHPBar;
    public Image staminaBar;

    bool isPaused;
    int enemiesRemaining;

    void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<playerController>();
        playerSpawnPos = GameObject.FindGameObjectWithTag("Player Spawn Pos");

        waveSpawner = GameObject.FindGameObjectWithTag("Wave Spawner");
        waveSpawnerScript = waveSpawner.GetComponent<WaveSpawner>();
    }


    void Update()
    {
        if (Input.GetButtonDown("Cancel") && activeMenu == null)
        {
            statePaused();
            activeMenu = pauseMenu;
            activeMenu.SetActive(isPaused);
        }
    }

    public void statePaused()
    {
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        isPaused = !isPaused;
    }

    public void stateUnpaused()
    {
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        isPaused = !isPaused;
        activeMenu.SetActive(false);
        activeMenu = null;
    }

    public void updateGameGoal(int amount)
    {
        enemiesRemaining += amount;
        enemiesRemainingText.text = enemiesRemaining.ToString("0");

        if(waveSpawnerScript.waveNumber % 5 == 0 && enemiesRemaining <=0 )
        {
            escape();
        }
    }

    public void youLose()
    {
        statePaused();
        activeMenu = loseMenu;
        activeMenu.SetActive(true);
    }

    public void escape()
    {
        statePaused();
        activeMenu = escapeMenu;
        activeMenu.SetActive(true);
    }
    
}
