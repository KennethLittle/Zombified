using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class gameManager : MonoBehaviour
{

    public static gameManager instance;
  
    public GameObject inventory;
    public InventoryUI inventoryUI;
    
    public LevelUpSystem levelUpSystem;
    public WaveManager waveManager;

    public enemyAI enemyAIScript;

    private UIManager uiManager;
    private PlayerManager playerManager;

    //public TextMeshProUGUI dialogueBox;
    //public TextMeshProUGUI input;
    //public TextMeshProUGUI npcName;

   
    public int enemiesKilled;
    public int totalXP;
    public bool isPaused;
    public int enemiesRemaining;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.LogError("Multiple instances of gameManager found. Destroying one.");
            Destroy(gameObject);
        }

        levelUpSystem = FindObjectOfType<LevelUpSystem>();
        waveManager= FindObjectOfType<WaveManager>();
        inventoryUI = FindObjectOfType<InventoryUI>();

        enemyAIScript = FindObjectOfType<enemyAI>();


        enemyAI.OnEnemyKilled += UpdateEnemiesKilled;
        UpdateTotalXP(totalXP);

        levelUpSystem.MarkRunStart();

        uiManager = GetComponent<UIManager>();
        playerManager = GetComponent<PlayerManager>();
    }


    void Update()
    {
      
    }

    public void updateGameGoal(int amount)
    {
        enemiesRemaining += amount;

        enemiesRemaining = Mathf.Max(enemiesRemaining, 0);
    }


    private void UpdateEnemiesKilled(int amount)
    {
        enemiesKilled += amount;
    }

    private void UpdateTotalXP(int amount)
    {
        gameManager.instance.levelUpSystem.totalAccumulatedXP = amount;
    }

    public void StartNewGame()
    {
        // Load the default data for a new game
        GameDataManager data = GameDataManager.GetDefaultGameData();

        // Instantiate a new player and get its script
        GameObject newPlayer = Instantiate(PlayerManager.instance.playerPrefab, PlayerManager.instance.playerSpawnPos.transform.position, Quaternion.identity);
        PlayerManager.instance.playerScript = newPlayer.GetComponent<playerController>();

        // Update player stats with data
        PlayerManager.instance.playerScript.defaultHP = data.playerHP;
        PlayerManager.instance.playerScript.defaultStamina = data.playerStamina;

        // Update game state with data
        this.enemiesKilled = data.enemiesKilled;
        this.totalXP = data.totalXP;
        this.enemiesRemaining = data.enemiesRemaining;

        // Ensure the rest of the systems know about the changes
        
    }

    public void SaveGame()
    {
        GameDataManager gameData = new GameDataManager(PlayerManager.instance.playerScript, this);
        SaveManager.Instance.SaveGame(PlayerManager.instance.playerScript, this);
    }

    public void LoadGame()
    {
        GameDataManager loadedData = SaveManager.Instance.LoadGame();

        // Update the current game's data based on the loaded data
        PlayerManager.instance.playerScript.defaultHP = loadedData.playerHP;
        PlayerManager.instance.playerScript.defaultStamina = loadedData.playerStamina;
        this.enemiesKilled = loadedData.enemiesKilled;
        this.totalXP = loadedData.totalXP;
        this.enemiesRemaining = loadedData.enemiesRemaining;

        // You might also need to set other values depending on what's included in your GameDataManager and what needs to be updated in the scene.
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
