using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpSystem : MonoBehaviour
{
    public PlayerStat playerStats; // Reference to the PlayerStatScript to access playerLevel

    public int totalAccumulatedXP;
    public int totalEarnedXP;
    public int requiredXP;

    // No need for extraHP and extraStamina variables. We'll calculate them directly in the LevelUp function

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

    public void RewardXPUponEscape()
    {
        int rewardXP = Mathf.FloorToInt(totalAccumulatedXP * 0.85f);
        gameManager.instance.totalXP += rewardXP;
        CheckLevelUp();
    }

    public void RewardXPUponDeath()
    {
        int rewardXP = Mathf.FloorToInt(totalAccumulatedXP * 0.15f);
        gameManager.instance.totalXP -= rewardXP;
    }

    // Move the MarkRunStart and MarkRunEnd functions to GameManager or a similar appropriate class.
}
