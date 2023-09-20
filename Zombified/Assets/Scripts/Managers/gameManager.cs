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
    public UIManager uiManager;
    private PlayerManager playerManager;
    private GameStateManager gameStateManager;
    public QuestManager questManager;


    // Game data
    [Header("Game Data")]
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
        questManager= FindObjectOfType<QuestManager>();
        uiManager = GetComponent<UIManager>();
        playerManager = GetComponent<PlayerManager>();
        gameStateManager= GetComponent<GameStateManager>();

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
            questManager.InitializeQuests();
            Debug.Log("Started First QUest");
        }
        // If you have default game states for enemies or other elements, you'd set them up here as well.
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
