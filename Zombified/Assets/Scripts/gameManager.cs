using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class gameManager : MonoBehaviour
{

    public static gameManager instance;
    public GameObject playerSpawnPos;

    public GameObject player;
    public playerController playerScript;

    public LevelUpSystem levelUpSystem;
    public WaveManager waveManager;

    public GameObject activeMenu;
    public GameObject pauseMenu;
    public GameObject loseMenu;
    public GameObject escapeMenu;
    public GameObject controlMenu;
    public TextMeshProUGUI enemiesRemainingText;
    public TextMeshProUGUI waveNumberText;
    public Image playerHPBar;
    public Image staminaBar;
    [SerializeField] GameObject playerDamageFlash;
    public TextMeshProUGUI ammoCur;
    public TextMeshProUGUI ammoMax;
    public TextMeshProUGUI medPackCur;
    public TextMeshProUGUI medPackMax;
    public TextMeshProUGUI ammoBoxAmount;
    public Image weaponIcon;



    bool isControlMenuActive = false;
    public int enemiesKilled;
    public int totalXP;
    bool isPaused;
    int enemiesRemaining;

    void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<playerController>();
        playerSpawnPos = GameObject.FindGameObjectWithTag("Player Spawn Pos");

        levelUpSystem = FindObjectOfType<LevelUpSystem>();
        waveManager= FindObjectOfType<WaveManager>();

        enemyAI.OnEnemyKilled += UpdateEnemiesKilled;
        UpdateTotalXP(totalXP);

        levelUpSystem.MarkRunStart();
    }


    void Update()
    {
        if (Input.GetButtonDown("Cancel") && activeMenu == null)
        {
            statePaused();
            activeMenu = pauseMenu;
            activeMenu.SetActive(isPaused);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            ToggleControlMenu();
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

        enemiesRemaining = Mathf.Max(enemiesRemaining, 0);
        enemiesRemainingText.text = enemiesRemaining.ToString("0");
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

    private void UpdateEnemiesKilled(int amount)
    {
        enemiesKilled += amount;
    }

    private void UpdateTotalXP(int amount)
    {
        gameManager.instance.levelUpSystem.totalAccumulatedXP = amount;
    }

    public IEnumerator playerFlashDamage()
    {
        playerDamageFlash.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        playerDamageFlash.SetActive(false);
    }

    private void ToggleControlMenu()
    {
        isControlMenuActive = !isControlMenuActive; 
        controlMenu.SetActive(isControlMenuActive);
    }

    public void ResetPauseState()
    {
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);

        }
    }

    public void ResetAndUnpauseGame()
    {
        stateUnpaused();
    }

    public void SaveGame()
    {
        SaveManager.SaveGame(this);
    }

    public void LoadGame()
    {
        SaveManager.LoadGame(this);
    }
}
