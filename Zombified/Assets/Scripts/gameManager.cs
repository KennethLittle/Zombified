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

    public GameObject player;
    public playerController playerScript;

    public GameObject activeMenu;
    public GameObject winMenu;
    public GameObject pauseMenu;
    public GameObject loseMenu;
    public GameObject levelSelector;
    public TextMeshProUGUI enemiesRemainingText;
    public Image playerHPBar;

    bool isPaused;
    int enemiesRemaining;

    void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<playerController>();
        playerSpawnPos = GameObject.FindGameObjectWithTag("Player Spawn Pos");
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

        if (enemiesRemaining <= 0)
        {
            youWin();
        }
    }

    public void youWin()
    {
        statePaused();
        activeMenu = winMenu;
        activeMenu.SetActive(true);
    }

    public void youLose()
    {
        statePaused();
        activeMenu = loseMenu;
        activeMenu.SetActive(true);
    }
    public void levelSelect()
    {
        statePaused();
        activeMenu = levelSelector;
        activeMenu.SetActive(true);
    }
}
