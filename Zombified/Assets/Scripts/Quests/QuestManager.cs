using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public List<Quest> quests;
    public int currentQuestIndex = 0;
    public Quest CurrentQuest => quests[currentQuestIndex];

    public DialogueManager dialogue;
    public GameObject dialogueText;


    public IEnumerator StartQuest()
    {
        // As soon as New game scene loads dialogue box opens, player gets the quests and the quest gets activated
        if (Input.GetButtonDown("StartNewGame"))
        {
            dialogueText.SetActive(true);
        }
        // Trigger dialogue for the first step
        // Code to trigger dialogue system with CurrentQuest.questSteps[0].stepDialogue

        while (currentQuestIndex < quests.Count)
        {
            yield return new WaitUntil(() => CurrentQuest.IsQuestComplete);
            currentQuestIndex++;

            if (currentQuestIndex < quests.Count)
            {
                // Start a delay before the next Quest begins
                yield return new WaitForSeconds(5.0f);

                // Trigger dialogue for the first step of the new quest
                // Code to trigger dialogue system with CurrentQuest.questStep[0].stepDialogue
            }
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
