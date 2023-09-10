using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KillGoal : QuestGoal
{
    public int EnemyID { get; set; }

    public KillGoal(int enemyID, string description, bool completed, int currentAmount, int requiredAmount)
    {
        this.EnemyID = enemyID;
        this.Description = description;
        this.Completed = completed;
        this.currentAmount = currentAmount;
        this.requiredAmount = requiredAmount;
    }

    public override void Init()
    {
        base.Init();

        System.Action<enemyAI> OnEnemyDeath = null;
        OnEnemyDeath += EnemyDied;
    }

    public void EnemyDied(enemyAI enemy) // listens for when an enemy dies
    {
        if (enemy.ID == this.EnemyID)
        {
            this.currentAmount++;
            Evaluate();
        }
    }
}
