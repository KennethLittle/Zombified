using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class QuestGiver : MonoBehaviour
{
    private void Start()
    {
        OpenQuestWindow();
    }

    public Quest quest;

    public playerController player;

    public GameObject questWindow;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI experienceText;
    public TextMeshProUGUI scrapText;

    public void OpenQuestWindow()
    {
        questWindow.SetActive(true);
        titleText.text = quest.title;
        descriptionText.text = quest.description;
        experienceText.text = quest.expReward.ToString();
        scrapText.text = quest.scrapReward.ToString();
    }

    public void AcceptQuest()
    {
        quest.isActive = true;
        player.quest = quest;
        questWindow.SetActive(false);
    }

    public void DenyQuest()
    {
        questWindow.SetActive(false);
    }
}