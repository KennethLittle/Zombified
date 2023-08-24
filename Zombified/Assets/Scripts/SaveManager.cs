using System.IO;
using UnityEngine;

public static class SaveManager
{
    private static readonly string savePath = Application.persistentDataPath + "/save.json";

    public static void SaveGame(gameManager manager)
    {
        GameData data = new GameData(manager);
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
    }

    public static void LoadGame(gameManager manager)
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
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
            Debug.LogWarning("Save file not found.");
        }
    }
}
