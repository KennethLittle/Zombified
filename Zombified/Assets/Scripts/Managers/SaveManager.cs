using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.Burst;
using Unity.VisualScripting;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public GameData GameData;
    

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
            questSaveDataList.Add(QuestDataConverter.ConvertQuestToSaveData(qr));
        }


        List<EnemyData> eD = (EnemyManager.Instance != null) ? EnemyManager.Instance.GetAllEnemyData() : new List<EnemyData>();

        // Save the game data
        GameData gameData = new GameData(pD, eD, questSaveDataList);
        string jsonData = JsonUtility.ToJson(gameData);
        File.WriteAllText(GetSavePath(saveSlot), jsonData);

        if (eD.Count == 0)
            Debug.Log("Game saved without enemy data.");
        else
            Debug.Log("Game saved with all data.");
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
        Debug.Log("Loading quest ID: " + data.activeQuestID);
        QuestManager.instance.SetCurrentQuestByID(data.activeQuestID);

        if (data.currentQueststepID > 0)
        {
            QuestManager.instance.SetCurrentQuestStepByID(data.currentQueststepID);
        }
        else
        {
            QuestManager.instance.InitializeQuests();  // Initialize quests for the first time
        }

        return data;
    }

    public class QuestDataConverter
    {
        public static QuestSaveData ConvertQuestToSaveData(QuestRuntime questRuntime)
        {
            QuestSaveData data = new QuestSaveData();

            // Use the questRuntime object that's passed in
            data.questID = questRuntime.questID;
            data.currentStepIndex = questRuntime.currentStepIndex;

            foreach (var step in questRuntime.blueprint.questSteps) // Assuming that blueprint has questSteps.
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
            stepData.stepID = questStep.stepID;

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
