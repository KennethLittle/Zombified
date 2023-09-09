using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestGoal
{
    public GoalType goalType;
    
    public string Description { get; set; }
    public bool Completed { get; set; }
    public int requiredAmount { get; set; }
    public int currentAmount { get; set; }

    public virtual void Init()
    {
        // default init stuff
    }

    public void Evaluate()
    {
        if (currentAmount >= requiredAmount)
        {
            Complete();
        }
    }

    public void Complete()
    {
        Completed = true;
    }

    public void EnemyKilled()
    {
        if(goalType == GoalType.Kill)
           currentAmount++;
    }

    public void ItemCollected()
    {
        if (goalType == GoalType.Collect)
            currentAmount++;
    }
}

public enum GoalType
{
    Kill,
    Collect
}