using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public bool isActive;
    public bool questComplete;

    public string title;
    public string description;
    public int expReward;
    public int scrapReward;


    public QuestGoal goal;

    public void Complete()
    {
        isActive = false;
    }
}

