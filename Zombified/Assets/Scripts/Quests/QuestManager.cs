using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public List<Quest> questTemplates; // Set these in the editor, these are your ScriptableObjects
    private List<QuestRuntime> quests = new List<QuestRuntime>();
    public int currentQuestIndex = 0;
    public QuestRuntime CurrentQuest => quests[currentQuestIndex];

    private void Awake()
    {
        InitializeQuests();
    }

    private void InitializeQuests()
    {
        foreach (Quest questTemplate in questTemplates)
        {
            quests.Add(new QuestRuntime(questTemplate));
        }
    }

    public void TriggerDialogue(Dialogue dialogue)
    {
        if (DialogueManager.instance != null)
        {
            DialogueManager.instance.StartDialogue(dialogue);
        }
        else
        {
            Debug.LogError("Dialogue Manager instance is not initialized.");
        }
    }

    public void NotifyItemFound(GameObject founditem)
    {
        QuestStepRuntime currentStep = CurrentQuest.CurrentStep;
        if (!currentStep.isCompleted && currentStep.blueprint is FindItemQuestStep findItemQuest)
        {
            findItemQuest.RegisterItemFound(founditem);
            currentStep.TryCompleteStep();
        }
    }

    public void NotifyEnemyKilled(GameObject killedEnemyType)
    {
        QuestStepRuntime currentStep = CurrentQuest.CurrentStep;
        if (!currentStep.isCompleted && currentStep.blueprint is KillEnemiesQuestStep killEnemyQuest)
        {
            killEnemyQuest.RegisterEnemyKill(killedEnemyType);
            currentStep.TryCompleteStep();
        }
    }

    public void NotifyObjectInteracted(GameObject interactedObjectType)
    {
        QuestStepRuntime currentStep = CurrentQuest.CurrentStep;
        if (!currentStep.isCompleted && currentStep.blueprint is InteractObjectQuestStep interObjQuest)
        {
            interObjQuest.RegisterObjectInteraction(interactedObjectType);
            currentStep.TryCompleteStep();
        }
    }

    public void NotifyReturnedHome()
    {
        QuestStepRuntime currentStep = CurrentQuest.CurrentStep;
        if (currentStep.blueprint is ReturnHomeQuestStep ReturnHomeQuest)
        {
            ReturnHomeQuest.ReturnHome();
            currentStep.TryCompleteStep();
        }
    }
}


