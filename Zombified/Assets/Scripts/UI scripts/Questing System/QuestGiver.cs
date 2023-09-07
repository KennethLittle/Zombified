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
    public Text descriptionText;
    public Text experienceText;
    public Text scrapText;

    public void OpenQuestWindow()
    {
        questWindow.SetActive(true);
        descriptionText.text = quest.description;
        experienceText.text =  quest.expReward.ToString();
        scrapText.text = quest.scrapReward.ToString();
    }
}
