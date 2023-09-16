using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public List<Quest> questTemplates; // Set these in the editor, these are your ScriptableObjects
    private List<QuestRuntime> quests = new List<QuestRuntime>();
    public int currentQuestIndex = 0;
    public QuestRuntime CurrentQuest => quests[currentQuestIndex];
    public QuestUIManager questUIManager;

    private void Awake()
    {
        InitializeQuests();

        foreach (QuestRuntime quest in quests)
        {
            quest.OnQuestCompleted += HandleQuestCompletion;
        }
    }

    private void InitializeQuests()
    {
        foreach (Quest questTemplate in questTemplates)
        {
            quests.Add(new QuestRuntime(questTemplate));
        }
    }

    public void ProgressToNextStepOrQuest()
    {
        if (CurrentQuest.IsQuestComplete)
        {
            // Move on to the next quest
            currentQuestIndex++;

            // Check if the new currentQuestIndex is within the bounds of the quests list
            if (currentQuestIndex < quests.Count)
            {
                // Start the next quest
                StartQuest();
            }
            else
            {
                // All quests are completed. Implement any logic you want here, for example:
                Debug.Log("All quests are completed!");
                // Or call a method like EndGame(), ShowCompletionCutscene(), etc.
            }
        }
        else
        {
            CurrentQuest.currentStepIndex++;
            CurrentQuest.CurrentStep.StartStep();
            OnQuestOrStepChanged();
        }
    }

    public void StartQuest()
    {
        // Reset the step index for the new quest
        CurrentQuest.currentStepIndex = 0;

        // Start the first step of the new quest
        CurrentQuest.CurrentStep.StartStep();
        OnQuestOrStepChanged();
    }


    public void HandleQuestCompletion(QuestRuntime completedQuest)
    {
        int nextQuestIndex = quests.IndexOf(completedQuest) + 1;
        if (nextQuestIndex < quests.Count)
        {
            currentQuestIndex = nextQuestIndex;
            quests[nextQuestIndex].CurrentStep.StartStep();
            OnQuestOrStepChanged();
        }
        else
        {
            // All quests completed. You can put end-game logic or whatever you want here.
        }
    }

    public void StartFirstQuest()
    {
        if (quests.Count > 0)
        {
            quests[0].CurrentStep.StartStep();
            OnQuestOrStepChanged();
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

    public void UpdateQuestUI()
    {
        if (CurrentQuest != null)
        {
            string questName = CurrentQuest.blueprint.questName;
            string questStepDescription = CurrentQuest.CurrentStep.blueprint.description;
            questUIManager.UpdateQuestUI(questName, questStepDescription);
        }
    }

    // Call this method whenever you progress to a new step or quest:
    public void OnQuestOrStepChanged()
    {
        UpdateQuestUI();
    }


public void NotifyItemFound(GameObject founditem)
    {
        QuestStepRuntime currentStep = CurrentQuest.CurrentStep;
        if (!currentStep.isCompleted && currentStep.blueprint is FindItemQuestStep findItemQuest)
        {
            findItemQuest.RegisterItemFound(founditem);
            currentStep.TryCompleteStep();

            if (currentStep.isCompleted)
            {
                CurrentQuest.ProgressToNextStepOrQuest();
            }
        }
    }

    public void NotifyEnemyKilled(GameObject killedEnemyType)
    {
        QuestStepRuntime currentStep = CurrentQuest.CurrentStep;
        if (!currentStep.isCompleted && currentStep.blueprint is KillEnemiesQuestStep killEnemyQuest)
        {
            killEnemyQuest.RegisterEnemyKill(killedEnemyType);
            currentStep.TryCompleteStep();
            if (currentStep.isCompleted)
            {
                CurrentQuest.ProgressToNextStepOrQuest();
            }
        }
    }

    public void NotifyObjectInteracted(GameObject interactedObjectType)
    {
        QuestStepRuntime currentStep = CurrentQuest.CurrentStep;
        if (!currentStep.isCompleted && currentStep.blueprint is InteractObjectQuestStep interObjQuest)
        {
            interObjQuest.RegisterObjectInteraction(interactedObjectType);
            currentStep.TryCompleteStep();
            if (currentStep.isCompleted)
            {
                CurrentQuest.ProgressToNextStepOrQuest();
            }
        }
    }

    public void NotifyReturnedHome()
    {
        QuestStepRuntime currentStep = CurrentQuest.CurrentStep;
        if (currentStep.blueprint is ReturnHomeQuestStep ReturnHomeQuest)
        {
            ReturnHomeQuest.ReturnHome();
            currentStep.TryCompleteStep();
            if (currentStep.isCompleted)
            {
                CurrentQuest.ProgressToNextStepOrQuest();
            }
        }
    }
}


