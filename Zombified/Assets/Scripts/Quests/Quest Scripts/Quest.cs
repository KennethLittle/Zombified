using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quests/Quest")]
public class Quest : ScriptableObject
{
    public int questID;
    public string questName;
    public List<QuestStep> questSteps; // Holds references to QuestStep ScriptableObjects
}


