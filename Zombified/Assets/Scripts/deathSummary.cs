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
        int enemiesKilled = gameManager.instance.enemiesKilled; // Use the correct variable name from your gameManager script
        int xpEarned = Mathf.FloorToInt(gameManager.instance.levelUpSystem.totalEarnedXP * 0.15f); // Keep 15% of the earned XP
        int playerLevel = gameManager.instance.levelUpSystem.playerLevel; // Access playerLevel from the LevelUpSystem script
        int extraHP = gameManager.instance.levelUpSystem.extraHP; // Access extraHP from the LevelUpSystem script
        int extraStamina = gameManager.instance.levelUpSystem.extraStamina; // Access extraStamina from the LevelUpSystem script
        int requiredXp = gameManager.instance.levelUpSystem.requiredXP; // Access requiredXP from the LevelUpSystem script
        int currentXP = Mathf.FloorToInt(gameManager.instance.levelUpSystem.totalAccumulatedXP * 0.15f);

        enemiesKilledText.text = "Enemies Killed: " + enemiesKilled.ToString();
        xpEarnedText.text = "XP Earned: " + xpEarned.ToString();
        playerLevelText.text = "Player Level: " + playerLevel.ToString();
        hpIncreaseText.text = "HP Increase: " + extraHP.ToString();
        staminaIncreaseText.text = "Stamina Increase: " + extraStamina.ToString();
        requiredXpText.text = "XP for Next Level: " + requiredXp.ToString();
        currentXPText.text = "Current XP: " + currentXP.ToString();

        gameManager.instance.SaveGame();
    }
}
