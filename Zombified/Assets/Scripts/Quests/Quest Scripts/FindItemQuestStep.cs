using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New FindItemQuestStep", menuName = "Quests/FindItemQuestStep")]
public class FindItemQuestStep : QuestStep
{
    public int requiredItemID;  // Change from GameObject to int
    public int requiredItemCount;
    public int currentItemFoundCount;

    public FindItemQuestStep(int itemIDToFind, int countToFind)
    {
        requiredItemID = itemIDToFind;
        requiredItemCount = countToFind;
        currentItemFoundCount = 0;
    }

    public void RegisterItemFound(int foundItemID)
    {
        if (!isCompleted && foundItemID == requiredItemID)
        {
            currentItemFoundCount++;
            isCompleted = CheckCompletion();
        }
    }

    public override bool CheckCompletion()
    {
        return currentItemFoundCount >= requiredItemCount;
    }
}