using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestStepRuntime
{
    public QuestStep blueprint; // The original QuestStep ScriptableObject
    public bool isCompleted;

    public QuestStepRuntime(QuestStep originalStep)
    {
        blueprint = originalStep;
        isCompleted = originalStep.isCompleted; // by default
    }

    public bool CheckCompletion() => blueprint.CheckCompletion();

    public void TryCompleteStep()
    {
        if (CheckCompletion())
        {
            isCompleted = true;
        }
    }
}