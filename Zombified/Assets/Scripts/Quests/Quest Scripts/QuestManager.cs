using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;
    public List<Quest> questTemplates;
    public List<QuestRuntime> quests = new List<QuestRuntime>();
    public QuestUIManager questUIManager;

    public int currentQuestIndex = 0;

    public QuestRuntime CurrentQuest => quests.Count > currentQuestIndex ? quests[currentQuestIndex] : null;

    public int currentQuestID { get; private set; }

    private void Awake()
    {
        InitializeSingleton();
    }

    private void InitializeSingleton()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void InitializeQuests()
    {
        foreach (Quest questTemplate in questTemplates)
        {
            quests.Add(new QuestRuntime(questTemplate, questTemplate.questID));
            UpdateQuestUI();
        }

        foreach (QuestRuntime quest in quests)
        {
            quest.OnQuestCompleted += HandleQuestCompletion;
        }

        Debug.Log($"Total quests initialized: {quests.Count}");

        if (quests.Count > 0 && quests[0].CurrentStep.blueprint.stepDialogue != null)
        {
            TriggerDialogue(quests[0].CurrentStep.blueprint.stepDialogue);
        }
    }

    public void ProgressToNextStepOrQuest()
    {
        if (CurrentQuest == null) return;

        // Check if the current step is the final step of the quest
        bool isFinalStep = CurrentQuest.currentStepIndex == CurrentQuest.stepsRuntime.Count - 1;
        Debug.Log($"Is final step of quest ID {CurrentQuest.questID}: {isFinalStep}");
        SaveManager.Instance.SaveGame();

        if (CurrentQuest.IsQuestComplete || isFinalStep)
        {
            HandleQuestCompletion(CurrentQuest);
        }
        else
        {
            CurrentQuest.ProgressToNextStepOrQuest();
            OnQuestOrStepChanged();
        }
    }

    public void HandleQuestCompletion(QuestRuntime completedQuest)
    {
        int nextQuestIndex = quests.IndexOf(completedQuest) + 1;
        if (nextQuestIndex < quests.Count)
        {
            Debug.Log($"Progressing from Quest {completedQuest.questID} to Quest {quests[nextQuestIndex].questID}");
            currentQuestIndex = nextQuestIndex;
            quests[nextQuestIndex].CurrentStep.StartStep();
            OnQuestOrStepChanged();
        }
        else
        {
            Debug.Log("All quests are completed!");
        }
    }

    public void OnQuestOrStepChanged()
    {
        if (CurrentQuest == null) return;

        Debug.Log($"Starting Step with ID: {CurrentQuest.CurrentStep.stepID} for Quest with ID: {CurrentQuest.questID}"); // Added log

        string questName = CurrentQuest.blueprint.questName;
        string questStepDescription = CurrentQuest.CurrentStep.blueprint.description;
        questUIManager.UpdateQuestUI(questName, questStepDescription);
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

        foreach (QuestSaveData questData in savedQuestData)
        {
            Quest correspondingTemplate = questTemplates.FirstOrDefault(q => q.questID == questData.questID);

            if (correspondingTemplate == null)
            {
                Debug.LogError($"No quest template found for questID: {questData.questID}");
                continue;
            }

            QuestRuntime loadedQuest = new QuestRuntime(correspondingTemplate, correspondingTemplate.questID);
            loadedQuest.currentStepIndex = questData.currentStepIndex;

            // Load the state of each step using the saved data
            for (int i = 0; i < questData.questStepSaveData.Count; i++)
            {
                QuestStepSaveData stepSaveData = questData.questStepSaveData[i];
                QuestStepRuntime step = loadedQuest.stepsRuntime[i];

                step.isCompleted = stepSaveData.isCompleted;

                // ... Load other necessary data for the step ...
            }

            quests.Add(loadedQuest);
        }

        // Update current quest index based on saved data
        currentQuestIndex = savedQuestData.FirstOrDefault(q => q.questID == currentQuestID)?.currentStepIndex ?? -1;

        // Update UI based on loaded quests
        OnQuestOrStepChanged();
    }

    public void SetCurrentQuestByID(int questID)
    {
        var questToSet = quests.FirstOrDefault(q => q.questID == questID);

        if (questToSet == null)
        {
            Debug.LogError("Quest with ID: " + questID + " not found.");
            return;
        }

        currentQuestIndex = quests.IndexOf(questToSet);
    }

    public void SetCurrentQuestStepByID(int stepID)
    {
        if (CurrentQuest == null)
        {
            Debug.LogError("No active quest to set the step for.");
            return;
        }

        // Ensure the stepID is within the bounds of the quest's steps
        if (stepID < 0 || stepID >= CurrentQuest.stepsRuntime.Count)
        {
            Debug.LogError("Step ID " + stepID + " is out of bounds for current quest.");
            return;
        }

        CurrentQuest.currentStepIndex = stepID;
        Debug.Log("Setting current step ID to: " + CurrentQuest.currentStepIndex);

        // Optionally, you can call any method here to update the UI or other systems.
        OnQuestOrStepChanged();
    }

    public int GetCurrentQuestID()
    {
        if (CurrentQuest != null)
        {
            return CurrentQuest.questID;
            // Assuming each QuestRuntime has a property or field named 'questID'
        }
        else
        {
            return -1; // Return an invalid value to indicate no current quest
        }
    }

    // This method returns the ID of the currently active quest step
    public int GetCurrentQuestStepID()
    {
        if (CurrentQuest != null && CurrentQuest.CurrentStep != null)
        {
            return CurrentQuest.CurrentStep.stepID;
            // Assuming each QuestStepRuntime has a property or field named 'stepID'
        }
        else
        {
            return -1; // Return an invalid value to indicate no current quest step
        }
    }


    public void NotifyItemFound(int foundItemID)
    {
        if (CurrentQuest == null) return;

        QuestStepRuntime currentStep = CurrentQuest.CurrentStep;

        if (!currentStep.isCompleted && currentStep.blueprint is FindItemQuestStep findItemQuest)
        {
            findItemQuest.RegisterItemFound(foundItemID);
            currentStep.TryCompleteStep();

            if (currentStep.isCompleted)
            {
                Debug.Log($"Step with ID: {currentStep.stepID} is completed using NotifyItemFound for Quest with ID: {CurrentQuest.questID}");
                ProgressToNextStepOrQuest();
                OnQuestOrStepChanged();
            }
            else
            {
                Debug.Log($"Item found but step with ID: {currentStep.stepID} for Quest with ID: {CurrentQuest.questID} is not yet complete.");
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
                OnQuestOrStepChanged();

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
                OnQuestOrStepChanged();
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
                OnQuestOrStepChanged();
            }
        }
    }

    public void NotifyGotToAlphaStation()
    {
        QuestStepRuntime currentStep = CurrentQuest.CurrentStep;
        if (currentStep.blueprint is GoToAlphaStationQuestStep GoToAlphaStationQuest)
        {
            currentStep.TryCompleteStep();
            if (currentStep.isCompleted)
            {
                CurrentQuest.ProgressToNextStepOrQuest();
                OnQuestOrStepChanged();
                GoToAlphaStationQuest.EnterAlphaStation();
            }
        }
    }

}


