using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestRuntime
{
    public int questID;
    public Quest blueprint;
    public int currentStepIndex = 0;
    public List<QuestStepRuntime> stepsRuntime = new List<QuestStepRuntime>();

    public event Action<QuestRuntime> OnQuestCompleted;

    public bool IsQuestComplete => currentStepIndex >= stepsRuntime.Count;

    public QuestStepRuntime CurrentStep => stepsRuntime[currentStepIndex];

    public QuestRuntime(Quest originalQuest, int id)
    {
        questID = id; // Assign the ID
        blueprint = originalQuest;
        foreach (var step in originalQuest.questSteps)
        {
            stepsRuntime.Add(new QuestStepRuntime(step, stepsRuntime.Count)); // Assigning IDs to steps based on their order
        }
    }

    public void ProgressToNextStepOrQuest()
    {
        if (currentStepIndex < stepsRuntime.Count - 1)  // Ensure we're not at the last step already
        {
            currentStepIndex++;
            CurrentStep.StartStep();
        }
        else if (IsQuestComplete)
        {
            OnQuestCompleted?.Invoke(this);
        }
    }

    public void ProceedToNextStep()
    {
        if (!IsQuestComplete)
        {
            currentStepIndex++;
        }
    }

    public QuestSaveData GetSaveData()
    {
        QuestSaveData saveData = new QuestSaveData()
        {
            questID = this.questID,  // Save the questID
            currentStepIndex = this.currentStepIndex
        };

        foreach (var stepRuntime in stepsRuntime)
        {
            saveData.questStepSaveData.Add(stepRuntime.GetSaveData());
        }

        return saveData;
    }

    public void LoadFromSaveData(QuestSaveData data)
    {
        this.questID = data.questID;

        for (int i = 0; i < data.questStepSaveData.Count; i++)
        {
            stepsRuntime[i].LoadFromSaveData(data.questStepSaveData[i]);
        }
    }
}
