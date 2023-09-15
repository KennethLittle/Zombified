using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Quest
{
    public string questName;
    public List<QuestStep> questSteps;
    public int currentStepIndex = 0;

    public bool IsQuestComplete => currentStepIndex >= questSteps.Count;

    public void ProceedToNextStep()
    {
        if(!IsQuestComplete) 
        {
            currentStepIndex++;
            // Trigger dialogue for the new step if there's more steps to go
            if(!IsQuestComplete)
            {
                // Code to trigger dialogue system with questSteps[currentStepIndex].stepDialogue
            }
        }
    }
}
