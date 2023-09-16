using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestUIManager : MonoBehaviour
{
    public Image questTracker;

    // Call this method to update the Quest and Step text
    public void UpdateQuestUI(string questName, string stepDescription)
    {
        questTracker.transform.Find("Quest Title").GetComponent<TextMeshProUGUI>().text = questName;
        questTracker.transform.Find("Quest Step").GetComponent<TextMeshProUGUI>().text = stepDescription;
    }
}
