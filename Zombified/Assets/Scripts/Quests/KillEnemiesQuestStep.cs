using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[CreateAssetMenu(fileName = "New KillEnemiesQuestStep", menuName = "Quests/KillEnemiesQuestStep")]
public class KillEnemiesQuestStep : QuestStep
{
    public GameObject EnemyPrefab;
    public int requiredKillCount;
    private int currentKillCount;

    public void RegisterEnemyKill(GameObject killedEnemyType)
    {
        if(!isCompleted && killedEnemyType ==  EnemyPrefab)
        {
            currentKillCount++;
            TryCompleteStep();
        }
    }

    public override bool CheckCompletion()
    {
        return currentKillCount >= requiredKillCount;
    }
}
