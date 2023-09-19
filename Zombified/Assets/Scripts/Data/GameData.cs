using System.Collections.Generic;
using System;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public PlayerData playerData;
    public List<EnemyData> enemiesData;
    public List<QuestSaveData> questsSaveData;
    public int activeQuestID;
    public int currentQueststepID;

    // Constructor that accepts PlayerData, List<EnemyData>, and List<QuestSaveData>
    public GameData(PlayerData pD, List<EnemyData> eD, List<QuestSaveData> qSD)
    {
        playerData = pD ?? new PlayerData();
        enemiesData = eD ?? new List<EnemyData>();
        questsSaveData = qSD ?? new List<QuestSaveData>();

        // Get current quest and step data from the QuestManager
        FetchQuestData();
    }

    private void FetchQuestData()
    {
        if (QuestManager.instance != null)
        {
            activeQuestID = QuestManager.instance.GetCurrentQuestID();
            currentQueststepID = QuestManager.instance.GetCurrentQuestStepID();
        }
        else
        {
            Debug.LogWarning("QuestManager instance is not available.");
            activeQuestID = -1; // Set to an invalid value to indicate an error
            currentQueststepID = -1; // Set to an invalid value to indicate an error
        }
    }
}