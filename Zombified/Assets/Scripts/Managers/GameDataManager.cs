[System.Serializable]
public class GameDataManager
{
    // Player Data
    public int playerHP;
    public float playerStamina;

    // Game State Data
    public int enemiesKilled;
    public int totalXP;
    public int enemiesRemaining;

    public GameDataManager()
    {

    }

    public GameDataManager(playerController player, gameManager game)
    {
        // Player Data
        playerHP = player.defaultHP;
        playerStamina = player.defaultStamina;
        

        // Game State Data
        enemiesKilled = game.enemiesKilled;
        totalXP = game.totalXP;
        enemiesRemaining = game.enemiesRemaining;
    }

    // Default data for a new game
    public static GameDataManager GetDefaultGameData()
    {
        return new GameDataManager
        {
            playerHP = 100,
            playerStamina = 100,
            enemiesKilled = 0,
            totalXP = 0,
            enemiesRemaining = 10
        };
    }
}