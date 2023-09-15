using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractObjectQuestStep : QuestStep 
{
    public GameObject objectType;

    public void RegisterObjectInteraction(GameObject interactedObjectType) 
    { 
        if(!isCompleted && interactedObjectType == objectType)
            TryCompleteStep();
    }

    public override bool CheckCompletion()
    { 
        return isCompleted;
    }
}
