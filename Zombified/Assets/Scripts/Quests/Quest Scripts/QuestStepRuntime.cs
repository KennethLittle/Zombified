using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestStepRuntime
{
    public QuestStep blueprint;
    public bool isCompleted;

    public QuestStepRuntime(QuestStep originalStep)
    {
        blueprint = originalStep;
    }

    public void StartStep()
    {
        if (blueprint.stepDialogue != null)
        {
            DialogueManager.instance.StartDialogue(blueprint.stepDialogue);
        }
        else
        {
            Debug.LogWarning("No dialogue set for quest step: " + blueprint.description);
        }
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