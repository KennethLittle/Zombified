using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public List<Quest> quests;
    public int currentQuestIndex = 0;
    public Quest CurrentQuest => quests[currentQuestIndex];


    public IEnumerator StartQuest()
    {
        // As soon as New game scene loads dialogue box opens, player gets the quests and the quest gets activated
        // Trigger dialogue for the first step
        // Code to trigger dialogue system
        if (Input.GetButtonDown("StartNewGame"))
        {
            TriggerDialogue(CurrentQuest.questSteps[0].stepDialogue);

            while (currentQuestIndex < quests.Count)
            {
                yield return new WaitUntil(() => CurrentQuest.IsQuestComplete);
                currentQuestIndex++;

                if (currentQuestIndex < quests.Count)
                {
                    // input delay before next quest begins
                    yield return new WaitForSeconds(5.0f);

                    // Trigger dialogue for the first step of next quest
                    TriggerDialogue(CurrentQuest.questSteps[0].stepDialogue);
                }
            }
        }
    }

    public void TriggerDialogue(Dialogue dialogue)
    {
        DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();
        if (dialogueManager != null)
        {
            dialogueManager.StartDialogue(dialogue);
        }
        else
        {
            Debug.LogError("Dialogue Manager not found in the scene.");
        }
    }

    public void NotifyItemFound(GameObject founditem)
    {
        if (CurrentQuest.currentStepIndex < CurrentQuest.questSteps.Count
            && CurrentQuest.questSteps[CurrentQuest.currentStepIndex] is FindItemQuestStep findItemQuest)
        {
            findItemQuest.RegisterItemFound(founditem);
        }
    }

    public void NotifyEnemyKilled(GameObject killedEnemyType)
    {
        if(CurrentQuest.currentStepIndex < CurrentQuest.questSteps.Count 
            && CurrentQuest.questSteps[CurrentQuest.currentStepIndex] is KillEnemiesQuestStep killEnemyQuest)
        {
            killEnemyQuest.RegisterEnemyKill(killedEnemyType);
        }
    }

    public void NotifyObjectInteracted(GameObject interactedObjectType)
    {
        if (CurrentQuest.currentStepIndex < CurrentQuest.questSteps.Count
            && CurrentQuest.questSteps[CurrentQuest.currentStepIndex] is InteractObjectQuestStep interObjQuest)
        {
            interObjQuest.RegisterObjectInteraction(interactedObjectType);
        }
    }

    public void NotifyReturnedHome() 
    {
        if(CurrentQuest.currentStepIndex == CurrentQuest.questSteps.Count
            && CurrentQuest.questSteps[CurrentQuest.currentStepIndex] is ReturnHomeQuestStep ReturnHomeQuest)
        {
            ReturnHomeQuest.ReturnHome();
        }
    }
}
