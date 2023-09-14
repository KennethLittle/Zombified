using static UnityEngine.Rendering.DebugUI.Table;
using UnityEditor.VersionControl;

[System.Serializable]
public class GameDataManager
{
    // Player Data
    public int playerHP;
    public float playerStamina;
    public int playerLevel;
    public int playerCurrentXP;
    public int playerRequiredXP;

    public GameDataManager() { }

    public GameDataManager(PlayerStat playerStats, LevelUpSystem levelSystem)
    {
        // Player Data
        playerHP = playerStats.HP;
        playerStamina = playerStats.stamina;
        playerLevel = playerStats.Level;
        playerCurrentXP = levelSystem.totalAccumulatedXP;
        playerRequiredXP = levelSystem.requiredXP;
    }

    // Default data for a new game
    public static GameDataManager GetDefaultGameData()
    {
        return new GameDataManager
        {
            playerHP = 100,
            playerStamina = 100,
            playerLevel = 1,
            playerCurrentXP = 0,
            playerRequiredXP = CalculateDefaultRequiredXP()
        };
    }

    // A method to calculate the default required XP for the first level up.
    // This is an example, Sir. You can adjust the formula as needed.
    private static int CalculateDefaultRequiredXP()
    {
        return 100; // The XP required for the first level up. Adjust this value or the formula as necessary.
    }
}