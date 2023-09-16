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
    public QuestManager questManager;

    // Game data
    [Header("Game Data")]
    public int enemiesKilled;
    public int totalXP;
    public int enemiesRemaining;
    public bool isPaused;
    public bool isInRun;

    // Initialization
    void Awake()
    {
        InitializeSingleton();
        SetupReferences();
    }

    void Start()
    {
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
        questManager= FindObjectOfType<QuestManager>();
        uiManager = GetComponent<UIManager>();
        playerManager = GetComponent<PlayerManager>();
        gameStateManager= GetComponent<GameStateManager>();

    }

    public void updateGameGoal(int amount)
    {
        enemiesRemaining += amount;
        enemiesRemaining = Mathf.Max(enemiesRemaining, 0); // Ensure enemiesRemaining doesn't go below 0
    }

    public void StartNewGame()
    {
        Debug.Log("StartNewGame called");
        // Set up a new game using default player data
        PlayerData defaultPlayerData = PlayerData.GetDefaultPlayerData();

        GameObject newPlayer = Instantiate(PlayerManager.instance.playerPrefab, PlayerManager.instance.playerSpawnPos.transform.position, Quaternion.identity);

        PlayerManager.instance.playerScript = newPlayer.GetComponent<playerController>();

        // Update game state with the default player data
        defaultPlayerData.LoadDataIntoPlayer(PlayerManager.instance);
        Debug.Log("QuestManager instance: " + questManager);
        if (questManager != null)
        {
            questManager.StartFirstQuest();
            Debug.Log("Started First QUest");
        }
        // If you have default game states for enemies or other elements, you'd set them up here as well.
    }

    public void SaveGameState(int saveSlot = 0)
    {
        SaveManager.Instance.SaveGame(saveSlot);
    }

    public void LoadGameState(int saveSlot = 0)
    {
        GameData loadedData = SaveManager.Instance.LoadGame(saveSlot);

        if (loadedData != null)
        {
            // Load player data
            loadedData.playerData.LoadDataIntoPlayer(PlayerManager.instance);

            // Load enemy data - You would need a method in EnemyManager to handle this.
            EnemyManager.Instance.LoadEnemyData(loadedData.enemiesData);
        }
        else
        {
            Debug.LogError("Failed to load game state.");
        }
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
        SaveGameState();
    }

    public void MarkRunEnd()
    {
        isInRun = false;
        SaveGameState();
    }
}
