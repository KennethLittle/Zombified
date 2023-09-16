using System.Collections.Generic;
using System;
using UnityEngine;

public class QuestRuntime
{
    public Quest blueprint; // The original Quest ScriptableObject
    public int currentStepIndex = 0;
    public List<QuestStepRuntime> stepsRuntime = new List<QuestStepRuntime>();
    public delegate void QuestCompletedHandler(QuestRuntime completedQuest);
    public event QuestCompletedHandler OnQuestCompleted;
    public QuestManager manager;

    public QuestRuntime(Quest originalQuest)
    {
        blueprint = originalQuest;
        foreach (var step in originalQuest.questSteps)
        {
            stepsRuntime.Add(new QuestStepRuntime(step));
        }
    }

    public bool IsQuestComplete => currentStepIndex >= stepsRuntime.Count;

    public QuestStepRuntime CurrentStep => stepsRuntime[currentStepIndex];

    public void ProgressToNextStepOrQuest()
    {
        if (IsQuestComplete)
        {
            manager.HandleQuestCompletion(this);
        }
        else
        {
            currentStepIndex++;
            CurrentStep.StartStep();
        }
    }
    public void CheckQuestCompletion()
    {
        if (IsQuestComplete)
        {
            OnQuestCompleted?.Invoke(this);
        }
    }
}
