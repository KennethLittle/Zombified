using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public Transform playerSpawnPos;
    public GameObject player;
    public GameObject playerPrefab;
    public playerController playerScript;
    public PlayerStat playerStat;
    public LevelUpSystem levelSystem;
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
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            SpawnPlayer();
        }
        else
        {
            playerScript = player.GetComponent<playerController>();
        }
    }

    public void SpawnPlayer()
    {
        player = Instantiate(playerPrefab, playerSpawnPos.position, Quaternion.identity);
        player.tag = "Player";
        playerScript = player.GetComponent<playerController>();
        playerStat = player.GetComponent<PlayerStat>();

        // Load temporary data if available
        if (TempPlayerData != null)
        {
            TempPlayerData.LoadDataIntoPlayer(this);
            TempPlayerData = null; // Clear temporary data after use
        }
    }
}
