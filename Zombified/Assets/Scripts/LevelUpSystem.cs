using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpSystem : MonoBehaviour
{
    public int playerLevel = 1;
    public int totalAccumulatedXP;
    public int totalEarnedXP;
    public int requiredXP;
    public int extraHP;
    public int extraStamina;
    
    

    private bool hasEscaped = false;
    public bool isInRun = false;

    public void GainXP(int xpAmount)
    {
        totalAccumulatedXP += xpAmount;
        totalEarnedXP += xpAmount;
        CheckLevelUp();
    }

    private void CheckLevelUp()
    {
        if (!isInRun) 
        {
            while (totalAccumulatedXP >= requiredXP)
            {
                LevelUp();

                requiredXP = CalculateRequiredXP();
            }
        }
        

    }

    public void LevelUp()
    {
        if (!isInRun) // Only apply HP and Stamina increase if not in a run
        {
            playerLevel++;
            totalAccumulatedXP -= requiredXP;

            extraHP = Mathf.RoundToInt(playerLevel * 15 * 1.75f);

            gameManager.instance.playerScript.IncreaseMaxHP(extraHP);

            extraStamina = Mathf.RoundToInt(playerLevel * 10 * 1.75f);

            gameManager.instance.playerScript.IncreaseMaxStamina(extraStamina);
        }


        requiredXP = CalculateRequiredXP();
    }


    private int CalculateRequiredXP()
    {
        return Mathf.RoundToInt(requiredXP * 1.7f);
    }


    public void RewardXPUponEscape()
    {
        int rewardXP = Mathf.FloorToInt(totalAccumulatedXP * 0.85f);
        gameManager.instance.totalXP += rewardXP;
        

        if (hasEscaped)
        {
            CheckLevelUp();
        }
    }

    public void MarkAsEscaped()
    {
        hasEscaped = true;
    }

    public void RewardXPUponDeath()
    {
        int rewardXP = Mathf.FloorToInt(totalAccumulatedXP * 0.15f);
        gameManager.instance.totalXP -= rewardXP;
        
        
    }

    public void MarkRunStart()
    {
        isInRun = true;
        
    }

    public void MarkRunEnd()
    {
        isInRun = false;
        
    }
}
