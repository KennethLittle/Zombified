using System.Collections;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private static SaveManager _instance;
    public static SaveManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SaveManager>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    _instance = obj.AddComponent<SaveManager>();
                }
            }
            return _instance;
        }
    }

    private static string savePath;
    public float autoSaveInterval = 600.0f;  // Default to 600 seconds (10 minutes)

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
            StartCoroutine(AutoSaveRoutine());
        }
        else
        {
            Destroy(this.gameObject);
        }

        if (string.IsNullOrEmpty(savePath))
        {
            savePath = Application.persistentDataPath + "/save.json";
        }
    }

    private IEnumerator AutoSaveRoutine()
    {
        while (true)  // Continuous loop
        {
            yield return new WaitForSeconds(autoSaveInterval);

            // Call the save function
            SaveGame(gameManager.instance);

            // If you have the save notification system as discussed earlier
            SaveNotification saveNotification = FindObjectOfType<SaveNotification>();
            if (saveNotification != null)
            {
                saveNotification.ShowSaveNotification();
            }
        }
    }

    public void SaveGame(gameManager manager)
    {
        GameData data = new GameData(manager);
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);

        // Display save notification
        SaveNotification saveNotification = FindObjectOfType<SaveNotification>();
        if (saveNotification != null)
        {
            saveNotification.ShowSaveNotification();
        }
    }

    public void LoadGame(gameManager manager)
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
            if (!string.IsNullOrEmpty(data.lastScene))
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(data.lastScene);
            }

            InventorySystem.Instance.items = data.inventoryItems;

        }
        else
        {
            Debug.LogWarning("Save file not found.");
        }
    }
}
