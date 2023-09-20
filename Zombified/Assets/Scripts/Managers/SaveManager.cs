using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public GameData GameData;
    private QuestManager questManager;
    private GameData tempLoadedData = null;
    private bool isNewGameTriggered = false;    

    private const string SAVE_FILENAME = "saveGame.json"; // Only one save file.
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

    public void SaveGame()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        PlayerData pD = new PlayerData(PlayerManager.instance);

        List<QuestSaveData> questSaveDataList = new List<QuestSaveData>();
        foreach (QuestRuntime qr in QuestManager.instance.quests)
        {
            questSaveDataList.Add(QuestDataConverter.ConvertQuestToSaveData(qr));
        }

        List<EnemyData> eD = (EnemyManager.Instance != null) ? EnemyManager.Instance.GetAllEnemyData() : new List<EnemyData>();

        // Save the game data
        GameData gameData = new GameData(pD, eD, questSaveDataList, currentScene);
        string jsonData = JsonUtility.ToJson(gameData);
        File.WriteAllText(GetSavePath(), jsonData);

    }

    public GameData LoadGame()
    {
        if (!File.Exists(GetSavePath()))
        {
            return null;
        }

        string jsonData = File.ReadAllText(GetSavePath());

        if (string.IsNullOrEmpty(jsonData))
        {
            return null;
        }

        GameData data = JsonUtility.FromJson<GameData>(jsonData);

        tempLoadedData = data;  // Assign to temporary data
        LoadSceneAndApplyData(data.sceneName);

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

    public void NewGame()
    {
        File.Delete(GetSavePath()); // Delete the current save file.

        isNewGameTriggered = true;

        SceneManager.LoadScene("HomeBase");
    }

    private void LoadSceneAndApplyData(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (isNewGameTriggered)
        {
            ApplyNewGameData();
            isNewGameTriggered = false; // Reset the flag.
        }
        else
        {
            ApplyLoadedData();
        }
    }



    private void ApplyLoadedData()
    {
        if (tempLoadedData != null)
        {
            // Your original logic to apply the loaded data goes here
            QuestManager.instance.SetCurrentQuestByID(tempLoadedData.activeQuestID);

            if (tempLoadedData.currentQueststepID > 0)
            {
                QuestManager.instance.SetCurrentQuestStepByID(tempLoadedData.currentQueststepID);
            }
            else
            {
                QuestManager.instance.InitializeQuests();  // Initialize quests for the first time
            }

            // Clear the temporary data
            tempLoadedData = null;
        }
    }

    private void ApplyNewGameData()
    {
        PlayerData defaultPlayerData = PlayerData.GetDefaultPlayerData();
        PlayerManager.instance.playerScript = PlayerManager.instance.player.GetComponent<playerController>();

        // Update game state with the default player data
        defaultPlayerData.LoadDataIntoPlayer(PlayerManager.instance);
        if (questManager != null)
        {
            questManager.InitializeQuests();
        }
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

    }

}
