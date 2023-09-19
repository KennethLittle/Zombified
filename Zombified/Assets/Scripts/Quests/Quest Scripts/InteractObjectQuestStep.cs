using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New InteractObjectQuestStep", menuName = "Quests/InteractObjectQuestStep")]
public class InteractObjectQuestStep : QuestStep 
{
    public GameObject objectType;

    public void RegisterObjectInteraction(GameObject interactedObjectType)
    {
        if (!isCompleted && interactedObjectType == objectType)
        {
            isCompleted = CheckCompletion();
        }
    }

    public override bool CheckCompletion()
    { 
        return isCompleted;
    }
}
