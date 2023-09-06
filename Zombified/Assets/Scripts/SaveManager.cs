using UnityEngine;

public static class SaveManager
{
    private static readonly string manualSaveKey = "gameDataManual";
    private static readonly string autoSaveKey = "gameDataAuto";

    public static void SaveGame(gameManager manager, bool isAutoSave = false)
    {
        GameData data = new GameData(manager);
        string json = JsonUtility.ToJson(data, true);
        string keyToUse = isAutoSave ? autoSaveKey : manualSaveKey;

        PlayerPrefs.SetString(keyToUse, json);
        PlayerPrefs.Save();
    }

    public static void LoadGame(gameManager manager, bool isAutoSave = false)
    {
        string keyToUse = isAutoSave ? autoSaveKey : manualSaveKey;

        if (PlayerPrefs.HasKey(keyToUse))
        {
            string json = PlayerPrefs.GetString(keyToUse);
            GameData data = JsonUtility.FromJson<GameData>(json);

            gameManager.instance.enemiesKilled = data.enemiesKilled;
            gameManager.instance.levelUpSystem.totalEarnedXP = data.totalEarnedXP;
            gameManager.instance.levelUpSystem.playerLevel = data.playerLevel;
            gameManager.instance.levelUpSystem.extraHP = data.extraHP;
            gameManager.instance.levelUpSystem.extraStamina = data.extraStamina;
            gameManager.instance.levelUpSystem.totalAccumulatedXP = data.totalAccumulatedXP;
        }
        else
        {
            Debug.LogWarning("Save data not found.");
        }
    }
}
