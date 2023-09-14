using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private const string SAVE_FILENAME = "savegame.json";

    public static SaveManager Instance { get; private set; }

    private void Awake()
    {
        // Singleton pattern
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
    }

    private string GetSavePath()
    {
        return Path.Combine(Application.persistentDataPath, SAVE_FILENAME);
    }

    public void SaveGame(PlayerStat playerStats, gameManager game)
    {
        GameDataManager data = new GameDataManager();

        // Player Data
        data.playerHP = playerStats.HP;
        data.playerStamina = playerStats.currentStamina;

        // Additional Game State Data
        // data.playerPosition = new Vector3Data(player.transform.position);
        // ... (add other game state data)

        // Convert to JSON and save
        string jsonData = JsonUtility.ToJson(data);
        File.WriteAllText(GetSavePath(), jsonData);

        Debug.Log("Game Saved!");
    }

    public GameDataManager LoadGame()
    {
        if (!File.Exists(GetSavePath()))
        {
            Debug.LogError("Save file not found!");
            return null;
        }

        string jsonData = File.ReadAllText(GetSavePath());

        // Error handling: Check if jsonData is null or empty
        if (string.IsNullOrEmpty(jsonData))
        {
            Debug.LogError("Save file is corrupted or empty!");
            return null;
        }

        GameDataManager data = JsonUtility.FromJson<GameDataManager>(jsonData);
        return data;
    }

    public bool DoesSaveGameExist()
    {
        return File.Exists(GetSavePath());
    }
}

// Optionally, you can create a struct to hold vector data for JSON serialization
[System.Serializable]
public struct Vector3Data
{
    public float x, y, z;

    public Vector3Data(Vector3 vec)
    {
        x = vec.x;
        y = vec.y;
        z = vec.z;
    }
}
