using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestUIManager : MonoBehaviour
{
    public Image questTracker;
    public QuestUIManager instance;

    // Call this method to update the Quest and Step text

    private void Start()
    {
        instance = this; 
    }
    public void UpdateQuestUI(string questName, string stepDescription, KillEnemiesQuestStep killQuest = null)
    {
        questTracker.transform.Find("Quest Title").GetComponent<TextMeshProUGUI>().text = questName;

        if (killQuest != null)
        {
            int currentKillCount = killQuest.GetCurrentKillCount();
            int requiredKills = killQuest.requiredKillCount;
            string formattedDescription = string.Format(stepDescription, currentKillCount, requiredKills);
            questTracker.transform.Find("Quest Step").GetComponent<TextMeshProUGUI>().text = formattedDescription;
        }
        else
        {
            questTracker.transform.Find("Quest Step").GetComponent<TextMeshProUGUI>().text = stepDescription;
        }
    }
}
