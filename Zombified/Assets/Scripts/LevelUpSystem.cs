using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpSystem : MonoBehaviour
{
    private PlayerStat playerStats; // Reference to the PlayerStatScript to access playerLevel

    public int totalAccumulatedXP;
    public int totalEarnedXP;
    public int requiredXP;

    

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

    public void LevelUp()
    {
        playerStats.Level++;
        totalAccumulatedXP -= requiredXP;

        int addedHP = Mathf.RoundToInt(playerStats.Level * 10 * Mathf.Log10(playerStats.Level + 10));
        PlayerManager.instance.playerScript.IncreaseMaxHP(addedHP);

        int addedStamina = Mathf.RoundToInt(playerStats.Level * 5 * Mathf.Log10(playerStats.Level + 10));
        PlayerManager.instance.playerScript.IncreaseMaxStamina(addedStamina);
    }

    private int CalculateRequiredXP()
    {
        return Mathf.RoundToInt(requiredXP * (1 + 0.1f * playerStats.Level));
    }


    
}
