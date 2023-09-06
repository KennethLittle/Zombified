[System.Serializable]
public class GameData
{
    public int enemiesKilled;
    public int totalEarnedXP;
    public int playerLevel;
    public int extraHP;
    public int extraStamina;
    public int totalAccumulatedXP;
    public int HP;
    public float Stamina;

    public GameData(gameManager manager)
    {
        enemiesKilled = gameManager.instance.enemiesKilled;
        totalEarnedXP = gameManager.instance.levelUpSystem.totalEarnedXP;
        playerLevel = gameManager.instance.levelUpSystem.playerLevel;
        extraHP = gameManager.instance.levelUpSystem.extraHP;
        extraStamina = gameManager.instance.levelUpSystem.extraStamina;
        totalAccumulatedXP = gameManager.instance.levelUpSystem.totalAccumulatedXP;
    }

    public void NewGame(gameManager manager)
    {
        enemiesKilled = 0;
        totalEarnedXP = 0;
        playerLevel = gameManager.instance.levelUpSystem.playerLevel;
        HP = gameManager.instance.playerScript.defaultHP;
        Stamina = gameManager.instance.playerScript.defaultStamina;
        totalAccumulatedXP = 0;
    }
}
