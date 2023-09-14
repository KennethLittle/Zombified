using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class gameManager : MonoBehaviour
{
    // Singleton
    public static gameManager instance;

    // Components and related scripts
    [Header("Components & Related Scripts")]
    public GameObject inventory;
    public InventoryUI inventoryUI;
    public LevelUpSystem levelUpSystem;
    private UIManager uiManager;
    private PlayerManager playerManager;
    private GameStateManager gameStateManager;

    // Game data
    [Header("Game Data")]
    public int enemiesKilled;
    public int totalXP;
    public int enemiesRemaining;
    public bool isPaused;
    public bool isInRun;

    // Initialization
    private void Awake()
    {
        InitializeSingleton();
        SetupReferences();
        UpdateTotalXP(totalXP);
        MarkRunStart();
    }

    private void InitializeSingleton()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Debug.LogError("Multiple instances of gameManager found. Destroying one.");
            Destroy(gameObject);
        }
    }

    private void SetupReferences()
    {
        levelUpSystem = FindObjectOfType<LevelUpSystem>();
        inventoryUI = FindObjectOfType<InventoryUI>();
        uiManager = GetComponent<UIManager>();
        playerManager = GetComponent<PlayerManager>();
        gameStateManager= GetComponent<GameStateManager>();

    }

    public void updateGameGoal(int amount)
    {
        enemiesRemaining += amount;
        enemiesRemaining = Mathf.Max(enemiesRemaining, 0); // Ensure enemiesRemaining doesn't go below 0
    }

    private void UpdateEnemiesKilled(int amount)
    {
        enemiesKilled += amount;
    }

    private void UpdateTotalXP(int amount)
    {
        levelUpSystem.totalAccumulatedXP = amount;
    }

    public void StartNewGame()
    {
        // Set up a new game using default data
        GameDataManager data = GameDataManager.GetDefaultGameData();

        GameObject newPlayer = Instantiate(PlayerManager.instance.playerPrefab, PlayerManager.instance.playerSpawnPos.transform.position, Quaternion.identity);

        PlayerManager.instance.playerScript = newPlayer.GetComponent<playerController>();

        // Update game state with data
        PlayerManager.instance.playerStat.HP = data.playerHP;
        PlayerManager.instance.playerStat.stamina = data.playerStamina;  // Assuming the player has stamina property in the PlayerStatScript.
        PlayerManager.instance.playerStat.Level = data.playerLevel;
        PlayerManager.instance.levelSystem.totalAccumulatedXP = data.playerCurrentXP;
        PlayerManager.instance.levelSystem.requiredXP = data.playerRequiredXP;
    }

    public void SaveGame()
    {
        SaveManager.Instance.SaveGame(PlayerManager.instance.playerStat, this);
    }

    public void LoadGame()
    {
        GameDataManager loadedData = SaveManager.Instance.LoadGame();

        PlayerManager.instance.playerStat.HP = loadedData.playerHP;
        PlayerManager.instance.playerStat.stamina = loadedData.playerStamina;
        PlayerManager.instance.playerStat.Level = loadedData.playerLevel;
        PlayerManager.instance.levelSystem.totalAccumulatedXP = loadedData.playerCurrentXP;
        PlayerManager.instance.levelSystem.requiredXP = loadedData.playerRequiredXP;
    }

    public void Defeat()
    {
        GameStateManager.instance.HandleDefeat();
    }

    public void Escape()
    {
        GameStateManager.instance.HandleEscape();
    }

    public void MarkRunStart()
    {
        isInRun = true;
        gameManager.instance.SaveGame();
    }

    public void MarkRunEnd()
    {
        isInRun = false;
        gameManager.instance.SaveGame();
    }
}
