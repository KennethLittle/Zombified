using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private const string SAVE_FILENAME = "savegame.json";

    public static SaveManager Instance { get; private set; }

    private void Awake()
    {
        // Ensure that there's only one SaveManager instance
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Rest of your initialization
    }

    // Get the path for saving/loading data
    private string GetSavePath()
    {
        return Path.Combine(Application.persistentDataPath, SAVE_FILENAME);
    }

    // Save the game data to a file
    public void SaveGame(playerController player, gameManager game)
    {
        GameDataManager data = new GameDataManager(player, game);

        string jsonData = JsonUtility.ToJson(data);
        File.WriteAllText(GetSavePath(), jsonData);

        Debug.Log("Game Saved!");
    }

    // Load game data from a file
    public GameDataManager LoadGame()
    {
        if (!File.Exists(GetSavePath()))
        {
            Debug.LogError("Save file not found!");
            return null;
        }

        string jsonData = File.ReadAllText(GetSavePath());
        GameDataManager data = JsonUtility.FromJson<GameDataManager>(jsonData);

        return data;
    }

    // Check if a save game exists
    public bool DoesSaveGameExist()
    {
        return File.Exists(GetSavePath());
    }
}
