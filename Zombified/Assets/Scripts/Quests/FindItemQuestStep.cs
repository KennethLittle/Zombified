using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FindItemQuestStep : QuestStep
{
    public GameObject requiredItem;
    private int requiredItemCount;
    private int currentItemFoundCount;

    public FindItemQuestStep(GameObject itemToFind, int countToFind)
    {
        requiredItem = itemToFind;
        requiredItemCount = countToFind;
        currentItemFoundCount = 0;
    }

    public void RegisterItemFound(GameObject foundItem)
    {
        if(!isCompleted && foundItem == requiredItem)
        {
            currentItemFoundCount++;
            TryCompleteStep();
        }
    }

    public override bool CheckCompletion()
    {
        return currentItemFoundCount >= requiredItemCount;
    }
}