using System.Linq;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public Transform playerSpawnPos;
    public GameObject player;
    public GameObject playerPrefab;
    public playerController playerScript;
    public PlayerStat playerStat;
    public static PlayerData TempPlayerData = null;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Debug.LogError("Multiple instances of PlayerManager found. Destroying one.");
            Destroy(gameObject);
        }

        
        // Spawn player if not found

    }

    private void Start()
    {
        SpawnPlayer();
    }

    public void SpawnPlayer(PlayerData data = null)
    {
        player = GameObject.FindGameObjectWithTag("Player");

        // If player does not exist, create one.
        if (player == null)
        {
            player = Instantiate(playerPrefab, playerSpawnPos.position, Quaternion.identity);
            player.tag = "Player";
            playerScript = player.GetComponent<playerController>();
            playerStat = player.GetComponent<PlayerStat>();
        }
        else
        {
            playerScript = player.GetComponent<playerController>();
            playerStat = player.GetComponent<PlayerStat>();
        }

        // Load data into the player.
        if (data != null)
        {
            // Load saved data into player.
            data.LoadDataIntoPlayer(this);
        }
        else
        {
            Debug.LogWarning("No player data provided. Using default values.");
            // Here, you can assign default values if needed.
        }

        QuestManager questManager = FindObjectOfType<QuestManager>();
        if (questManager != null)
        {
            // Check if questsSaveData exists and if it's not empty.
            if (data != null && data.questsSaveData != null && data.questsSaveData.Any())
            {
                // Load the quest by its ID.
                int questID = data.currentQuestID;
                if (questID >= 0) // Assuming that valid quest IDs are non-negative.
                {
                    questManager.SetCurrentQuestByID(questID);
                    questManager.SetCurrentQuestStepByID(data.currentQuestStepIndex);
                }
                else
                {
                    Debug.LogError("Invalid quest ID found in saved data.");
                }
            }
            else
            {
                questManager.InitializeQuests(); // Initialize quests for the first time
                Debug.Log("Quest Initialized");
            }
        }
    }
}
