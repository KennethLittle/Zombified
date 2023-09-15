using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public List<Quest> quests;
    public int currentQuestIndex = 0;
    public Quest CurrentQuest => quests[currentQuestIndex];

    public IEnumerator StartQuest()
    {
        // Start the first quest when the player clicks the Newgame button and the player spawns in
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

    // Other methods to notify quest steps like RegisterEnemyKill, RegisterObjectInteraction, etc.
}
