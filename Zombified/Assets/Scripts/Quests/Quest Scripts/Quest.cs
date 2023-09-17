using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quests/Quest")]
public class Quest : ScriptableObject
{
    public string questID;
    public string questName;
    public List<QuestStep> questSteps; // Holds references to QuestStep ScriptableObjects
    public int currentStepIndex = 0;

    public bool IsQuestComplete => currentStepIndex >= questSteps.Count;

    public void ProceedToNextStep()
    {
        if (!IsQuestComplete)
        {
            currentStepIndex++;
        }
    }
}


