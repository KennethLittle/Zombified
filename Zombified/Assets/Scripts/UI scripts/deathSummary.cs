using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class deathSummary : MonoBehaviour
{
    public TextMeshProUGUI enemiesKilledText;
    public TextMeshProUGUI xpEarnedText;
    public TextMeshProUGUI playerLevelText;
    public TextMeshProUGUI hpIncreaseText;
    public TextMeshProUGUI staminaIncreaseText;
    public TextMeshProUGUI requiredXpText;
    public TextMeshProUGUI currentXPText;

    private void Start()
    {
        int enemiesKilled = gameManager.instance.enemiesKilled;
        int xpEarned = Mathf.FloorToInt(gameManager.instance.levelUpSystem.totalEarnedXP * 0.15f);
        int playerLevel = PlayerManager.instance.playerScript.playerStat.Level; // Access playerLevel from PlayerStatScript
        int totalHP = PlayerManager.instance.playerScript.playerStat.HP; // Access total HP from PlayerStatScript
        int totalStamina = Mathf.FloorToInt(PlayerManager.instance.playerScript.playerStat.stamina); // Access total Stamina from PlayerStatScript
        int requiredXp = gameManager.instance.levelUpSystem.requiredXP;
        int currentXP = gameManager.instance.levelUpSystem.totalAccumulatedXP;

        enemiesKilledText.text = "Enemies Killed: " + enemiesKilled.ToString();
        xpEarnedText.text = "XP Earned: " + xpEarned.ToString();
        playerLevelText.text = "Player Level: " + playerLevel.ToString();
        hpIncreaseText.text = "Total HP: " + totalHP.ToString();
        staminaIncreaseText.text = "Total Stamina: " + totalStamina.ToString();
        requiredXpText.text = "XP for Next Level: " + requiredXp.ToString();
        currentXPText.text = "Current XP: " + currentXP.ToString();

        gameManager.instance.SaveGame();
    }
}
