using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpSystem : MonoBehaviour
{
    public int playerLevel = 1;
    public int totalAccumulatedXP;
    public int totalEarnedXP;
    public int requiredXP;
    

    private bool hasEscaped = false;

    public void GainXP(int xpAmount)
    {
        totalAccumulatedXP += xpAmount;
        totalEarnedXP += xpAmount;
        CheckLevelUp();
    }

    private void CheckLevelUp()
    {
        while (totalAccumulatedXP >= requiredXP)
        {
            LevelUp();

            requiredXP = CalculateRequiredXP();
        }
    }

    private void LevelUp()
    {
        playerLevel++;
        totalAccumulatedXP -= requiredXP;

        int extraHP = ExtraHP();
        gameManager.instance.playerScript.IncreaseMaxHP(extraHP);
        int extraStamina = ExtraStamina();
        gameManager.instance.playerScript.IncreaseMaxStamina(extraStamina);

        requiredXP = CalculateRequiredXP();
    }

    public int ExtraHP()
    {
        return Mathf.RoundToInt(playerLevel * 10 * 1.15f);
    }

    public int ExtraStamina()
    {
        return Mathf.RoundToInt(playerLevel * 15 * 1.25f);
    }

    private int CalculateRequiredXP()
    {
        return Mathf.RoundToInt(requiredXP * 1.7f);
    }


    public void RewardXPUponEscape()
    {
        int rewardXP = Mathf.FloorToInt(totalAccumulatedXP * 0.85f);
        gameManager.instance.totalXP += rewardXP;
        totalAccumulatedXP -= rewardXP;

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
        totalAccumulatedXP -= rewardXP;
    }
}
