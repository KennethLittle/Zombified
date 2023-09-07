using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class QuestGiver : MonoBehaviour
{
    public Quest quest;

    public playerController player;

    public GameObject questWindow;
    public Text titleText;
    public Text descriptionText;
    public Text experienceText;
    public Text scrapText;

    public void OpenQuestWindow()
    {
        questWindow.SetActive(true);
        titleText.text = quest.title;
        descriptionText.text = quest.description;
        experienceText.text = quest.expReward.ToString();
        scrapText.text = quest.scrapReward.ToString();
    }

    //public void AcceptQuest(List<Quest> quest, int i)
    //{
    //    questWindow.SetActive(false);
    //    quest[i].isActive = true;
    //    player.quest = quest; // give quest to player
    //}

    public void AcceptQuest()
    {
        questWindow.SetActive(false);
        quest.isActive = true;
        player.quest = quest; // give quest to player
    }
}
