using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private const string SAVE_FILENAME = "newSave{0}.json";
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

    private string GetSavePath(int saveSlot)
    {
        return Path.Combine(Application.persistentDataPath, string.Format(SAVE_FILENAME, saveSlot));
    }

    public void SaveGame(int saveSlot = 0)
    {
        PlayerData pD = new PlayerData(PlayerManager.instance);
        List<EnemyData> eD = EnemyManager.Instance.GetAllEnemyData();

        GameData gameData = new GameData(pD, eD);

        string jsonData = JsonUtility.ToJson(gameData);
        File.WriteAllText(GetSavePath(saveSlot), jsonData);

        Debug.Log("Game Saved!");
    }

    public GameData LoadGame(int saveSlot)
    {
        if (!File.Exists(GetSavePath(saveSlot)))
        {
            Debug.LogError("Save file not found!");
            return null;
        }

        string jsonData = File.ReadAllText(GetSavePath(saveSlot));

        if (string.IsNullOrEmpty(jsonData))
        {
            Debug.LogError("Save file is corrupted or empty!");
            return null;
        }

        GameData data = JsonUtility.FromJson<GameData>(jsonData);
        return data;
    }

    public bool DoesSaveGameExist(int saveSlot)
    {
        return File.Exists(GetSavePath(saveSlot));
    }

    // List all save files
    public List<string> GetAllSaveFiles()
    {
        var allSaveFiles = Directory.GetFiles(Application.persistentDataPath, "*.json")
                                     .Select(Path.GetFileName)
                                     .ToList();
        return allSaveFiles;
    }

    public int GetNextSaveSlot()
    {
        List<string> saveFiles = GetAllSaveFiles();
        int maxSlotNumber = 0;

        foreach (string saveFile in saveFiles)
        {
            string slotString = saveFile.Replace("newSave", "").Replace(".json", "");
            if (int.TryParse(slotString, out int currentSlot))
            {
                if (currentSlot > maxSlotNumber)
                {
                    maxSlotNumber = currentSlot;
                }
            }
        }

        return maxSlotNumber + 1;
    }

    public void RenameSaveFile(int saveSlot, string newName)
    {
        string oldPath = GetSavePath(saveSlot);
        string newPath = Path.Combine(Application.persistentDataPath, newName + ".json");

        if (File.Exists(oldPath))
        {
            File.Move(oldPath, newPath);
        }
        else
        {
            Debug.LogError("File doesn't exist: " + oldPath);
        }
    }

}
