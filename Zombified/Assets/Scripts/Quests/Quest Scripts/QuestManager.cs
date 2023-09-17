using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;
    public List<Quest> questTemplates; // Set these in the editor, these are your ScriptableObjects
    public List<QuestRuntime> quests = new List<QuestRuntime>();
    public int currentQuestIndex = 0;
    public QuestRuntime CurrentQuest
    {
        get
        {
            if (currentQuestIndex < 0 || currentQuestIndex >= quests.Count)
            {
                Debug.LogError("CurrentQuest access error. Index out of range: " + currentQuestIndex);
                return null;
            }
            return quests[currentQuestIndex];
        }
    }
    public QuestUIManager questUIManager;

    private void Awake()
    {
        InitializeQuests();
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Debug.LogError("Multiple instances of QuestManager found. Destroying one.");
            Destroy(gameObject);
        }

        

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

        Debug.Log("Total quests initialized: " + quests.Count);
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
        if (currentQuestIndex >= quests.Count || currentQuestIndex < 0)
        {
            Debug.LogError("Current quest index out of bounds: " + currentQuestIndex);
            return;
        }
        // Reset the step index for the new quest
        CurrentQuest.currentStepIndex = 0;
        Debug.Log("Quests are available. Starting the first one.");
        // Start the first step of the new quest
        CurrentQuest.CurrentStep.StartStep();
        OnQuestOrStepChanged();
        SaveManager.Instance.SaveGame();
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
        Debug.Log("Inside StartFirstQuest method");
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
            Debug.Log("Triggering dialogue for " + dialogue.speakerName);
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
            Debug.Log("Updating Quest UI with quest name: " + questName);
        }
    }

    public void LoadQuests(List<QuestSaveData> savedQuestData)
    {
        if (savedQuestData == null || savedQuestData.Count == 0)
        {
            Debug.LogError("Invalid saved quest data provided.");
            return;
        }

        // Clear any existing quests
        quests.Clear();

        // Repopulate quests based on saved data
        for (int i = 0; i < savedQuestData.Count; i++)
        {
            // Assuming the order of saved quests matches the order in questTemplates
            if (i >= questTemplates.Count)
            {
                Debug.LogError("Mismatch in number of saved quests and quest templates.");
                break;
            }

            Quest questTemplate = questTemplates[i];

            // Create a new runtime quest instance based on the saved data and template
            QuestRuntime loadedQuest = new QuestRuntime(questTemplate);

            // Here you populate the data from savedQuestData[i] into loadedQuest 
            // For example:
            loadedQuest.currentStepIndex = savedQuestData[i].currentStepIndex;
            //... Load other necessary data ...

            quests.Add(loadedQuest);
        }

        // Update current quest index based on saved data (if you've saved the current quest index)
        // currentQuestIndex = ...;

        // Optional: Trigger some UI or other systems to update based on the loaded quests
        OnQuestOrStepChanged();
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


