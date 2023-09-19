using UnityEngine;

public class QuestStepRuntime
{
    public QuestStep blueprint;
    public bool isCompleted;
    public int stepID;

    public QuestStepRuntime(QuestStep originalStep, int id)
    {
        stepID = id; // Assign the ID
        blueprint = originalStep;
    }

    public void StartStep()
    {
        if (blueprint.stepDialogue != null && DialogueManager.instance != null)
        {
            DialogueManager.instance.StartDialogue(blueprint.stepDialogue);
        }
        else if (blueprint.stepDialogue == null)
        {
            Debug.LogWarning($"No dialogue set for quest step: {blueprint.description}");
        }
    }

    public bool CheckCompletion() => blueprint.CheckCompletion();

    public void TryCompleteStep()
    {
        Debug.Log($"Checking if Step with ID: {this.stepID} can be completed.");

        // The conditions to complete the step
        if (true)
        {
            this.isCompleted = true;
            Debug.Log($"Successfully completed Step with ID: {this.stepID}.");
        }
        else
        {
            Debug.Log($"Failed to complete Step with ID: {this.stepID}. Conditions not met.");
        }
    }

    public QuestStepSaveData GetSaveData()
    {
        return new QuestStepSaveData()
        {
            stepID = this.stepID,  // Save the stepID
            isCompleted = this.isCompleted
            // Add other fields as needed
        };
    }

    public void LoadFromSaveData(QuestStepSaveData data)
    {
        this.stepID = data.stepID;
        // Load other fields as needed
    }
}