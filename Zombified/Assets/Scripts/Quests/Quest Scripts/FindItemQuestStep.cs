using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New FindItemQuestStep", menuName = "Quests/FindItemQuestStep")]
public class FindItemQuestStep : QuestStep
{
    public int requiredItemID;  // Change from GameObject to int
    public int requiredItemCount;
    public int currentItemFoundCount;
    public GameObject objectType;

    public FindItemQuestStep(int itemIDToFind, int countToFind)
    {
        requiredItemID = itemIDToFind;
        requiredItemCount = countToFind;
        currentItemFoundCount = 0;
    }

    public void RegisterItemFound(int foundItemID)
    {
        Debug.Log($"RegisterItemFound called with ID: {foundItemID}, Required ID: {requiredItemID}");
        if (!isCompleted && foundItemID == requiredItemID)
        {
            currentItemFoundCount++;
            Debug.Log($"Current Item Count: {currentItemFoundCount}, Required Item Count: {requiredItemCount}");
            isCompleted = CheckCompletion();
            if (isCompleted)
            {
                Debug.Log("Quest Step Completed.");
            }
        }
    }

    public override bool CheckCompletion()
    {
        return currentItemFoundCount >= requiredItemCount;
    }

    public void StartQuestStep()
    {
        GameObject questItem = QuestItemManager.instance.questItems[requiredItemID];
        if (questItem != null)
        {
            questItem.SetActive(true);
        }
    }

    public void EndQuestStep()
    {
        GameObject questItem = QuestItemManager.instance.questItems[requiredItemID];
        if (questItem != null)
        {
            questItem.SetActive(false);
        }
    }
}