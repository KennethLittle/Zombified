using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
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

        List<QuestSaveData> questSaveDataList = new List<QuestSaveData>();
        foreach (QuestRuntime qr in QuestManager.instance.quests)
        {
            questSaveDataList.Add(qr.GetSaveData());
        };

        // Check if the EnemyManager instance exists
        if (EnemyManager.Instance == null)
        {
            Debug.LogWarning("No EnemyManager instance found. Saving without enemy data.");

            // Save just the player data without enemy data
            GameData gameData = new GameData(pD, new List<EnemyData>(), new List<QuestSaveData>());
            string jsonData = JsonUtility.ToJson(gameData);
            File.WriteAllText(GetSavePath(saveSlot), jsonData);
            Debug.Log("Game saved without enemy data.");
            return;
        }

        List<EnemyData> eD = EnemyManager.Instance.GetAllEnemyData();
        // Check if there's any enemy data to save
        if (eD == null || eD.Count == 0)
        {
            Debug.LogWarning("No enemy data to save. Saving without enemy data.");

            // Save just the player data without enemy data
            GameData gameData = new GameData(pD, new List<EnemyData>(), questSaveDataList);
            string jsonData = JsonUtility.ToJson(gameData);
            File.WriteAllText(GetSavePath(saveSlot), jsonData);
            Debug.Log("Game saved without enemy data.");
            return;
        }


        // If we have both player and enemy data, save them together
        GameData gameDataWithEnemies = new GameData(pD, eD, questSaveDataList);
        string jsonDataWithEnemies = JsonUtility.ToJson(gameDataWithEnemies);
        File.WriteAllText(GetSavePath(saveSlot), jsonDataWithEnemies);
        Debug.Log("Game Saved with enemy and quest data!");
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

    public class QuestDataConverter
    {
        public static QuestSaveData ConvertQuestToSaveData(Quest quest)
        {
            QuestSaveData data = new QuestSaveData();

            data.currentStepIndex = quest.currentStepIndex;

            foreach (var step in quest.questSteps)
            {
                QuestStepSaveData stepData = ConvertQuestStepToSaveData(step);
                data.questStepSaveData.Add(stepData);
            }

            return data;
        }

        private static QuestStepSaveData ConvertQuestStepToSaveData(QuestStep questStep)
        {
            QuestStepSaveData stepData = new QuestStepSaveData();
            stepData.description = questStep.description;
            stepData.isCompleted = questStep.isCompleted;

            // If I decide to save the Dialogue as mentioned above:
            // stepData.stepDialogueSaveData = ConvertDialogueToSaveData(questStep.stepDialogue);

            return stepData;
        }

        // If I decide to save Dialogue:
        // private static DialogueSaveData ConvertDialogueToSaveData(Dialogue dialogue)
        // {
        //     DialogueSaveData dialogueData = new DialogueSaveData();
        //     // Fill in the details
        //     return dialogueData;
        // }
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
