using System.Collections.Generic;

[System.Serializable]
public class QuestSaveData
{
    public int questID;
    public int currentStepIndex;
    public List<QuestStepSaveData> questStepSaveData = new List<QuestStepSaveData>();
    // Add any other fields needed to save the state of a quest.
}