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
    public WaveManager waveManager;
    private UIManager uiManager;
    private PlayerManager playerManager;

    // Game data
    [Header("Game Data")]
    public int enemiesKilled;
    public int totalXP;
    public int enemiesRemaining;
    public bool isPaused;

    // Initialization
    private void Awake()
    {
        InitializeSingleton();
        SetupReferences();
        UpdateTotalXP(totalXP);
        levelUpSystem.MarkRunStart();
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
        waveManager = FindObjectOfType<WaveManager>();
        inventoryUI = FindObjectOfType<InventoryUI>();
        uiManager = GetComponent<UIManager>();
        playerManager = GetComponent<PlayerManager>();
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
        PlayerManager.instance.playerScript.defaultHP = data.playerHP;
        PlayerManager.instance.playerScript.defaultStamina = data.playerStamina;
        enemiesKilled = data.enemiesKilled;
        totalXP = data.totalXP;
        enemiesRemaining = data.enemiesRemaining;
    }

    public void SaveGame()
    {
        SaveManager.Instance.SaveGame(PlayerManager.instance.playerScript, this);
    }

    public void LoadGame()
    {
        GameDataManager loadedData = SaveManager.Instance.LoadGame();
        PlayerManager.instance.playerScript.defaultHP = loadedData.playerHP;
        PlayerManager.instance.playerScript.defaultStamina = loadedData.playerStamina;
        enemiesKilled = loadedData.enemiesKilled;
        totalXP = loadedData.totalXP;
        enemiesRemaining = loadedData.enemiesRemaining;
    }

    public void Defeat()
    {
        GameStateManager.instance.HandleDefeat();
    }

    public void Escape()
    {
        GameStateManager.instance.HandleEscape();
    }
}
