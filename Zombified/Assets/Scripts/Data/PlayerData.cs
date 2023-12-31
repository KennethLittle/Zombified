using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public Vector3 playerPosition;
    public Quaternion playerRotation;
    public int currentQuestID;
    public LocationData.Location currentLocation;
    public List<string> currentInventory; // List of items' names or IDs
    public List<QuestSaveData> questsSaveData;

    public int currentQuestIndex; // index of the current quest in the QuestManager's list
    public int currentQuestStepIndex;

    // Player stats fields
    public int HP;
    public int HPMax;
    public float stamina;
    public float currentStamina;
    public float staminaConsumptionRate;
    public float staminaRegenerationRate;
    public int Level;
    public float playerSpeed;
    public float sprintMod;
    public int jumpMax;
    public float jumpHeight;
    public float gravityValue;

    public PlayerData() { }

    public PlayerData(PlayerManager playerManager)
    {
        PlayerStat stats = playerManager.playerStat;

        playerPosition = playerManager.player.transform.position;
        playerRotation = playerManager.player.transform.rotation;

        HP = stats.HP;
        HPMax = stats.HPMax;
        stamina = stats.stamina;
        currentStamina = stats.currentStamina;
        staminaConsumptionRate = stats.staminaConsumptionRate;
        staminaRegenerationRate = stats.staminaRegenerationRate;
        Level = stats.Level;
        playerSpeed = stats.playerSpeed;
        sprintMod = stats.sprintMod;
        jumpMax = stats.jumpMax;
        jumpHeight = stats.jumpHeight;
        gravityValue = stats.gravityValue;

        QuestManager questManager = QuestManager.instance;
        if (questManager != null)
        {
            currentQuestIndex = questManager.currentQuestIndex;
            if (questManager.CurrentQuest != null)
            {
                currentQuestStepIndex = questManager.CurrentQuest.currentStepIndex;

                questsSaveData = new List<QuestSaveData>();

                foreach (var quest in questManager.quests)
                {
                    QuestSaveData questData = new QuestSaveData();
                    questData.questID = quest.questID;
                    questData.currentStepIndex = quest.currentStepIndex;

                    foreach (var step in quest.stepsRuntime)
                    {
                        QuestStepSaveData stepData = new QuestStepSaveData
                        {
                            isCompleted = step.isCompleted,
                            stepID = step.stepID,
                            // ... Set other properties of the step ...
                        };

                        questData.questStepSaveData.Add(stepData);
                    }

                    questsSaveData.Add(questData);
                }
            }
        }
    }

    public void LoadDataIntoPlayer(PlayerManager playerManager)
    {
        playerManager.player.transform.position = playerPosition;
        playerManager.player.transform.rotation = playerRotation;
        PlayerStat stats = playerManager.playerStat;

        stats.HP = HP;
        stats.HPMax = HPMax;
        stats.stamina = stamina;
        stats.currentStamina = currentStamina;
        stats.staminaConsumptionRate = staminaConsumptionRate;
        stats.staminaRegenerationRate = staminaRegenerationRate;
        stats.Level = Level;
        stats.playerSpeed = playerSpeed;
        stats.sprintMod = sprintMod;
        stats.jumpMax = jumpMax;
        stats.jumpHeight = jumpHeight;
        stats.gravityValue = gravityValue;

        QuestManager questManager = QuestManager.instance;
        if (questManager != null)
        {
            questManager.SetCurrentQuestByID(currentQuestID); // Assuming you write this method to set a quest by its ID
        }

        // When you implement the quest, location, and inventory systems, 
        // you'll similarly use the PlayerManager to set the player's state based on the saved data.
    }

    // Default data for a new game
    public static PlayerData GetDefaultPlayerData()
    {
        return new PlayerData
        {
            playerPosition = Vector3.zero, // Default position

            HP = 100,
            HPMax = 100,
            stamina = 100,
            currentStamina = 100,
            staminaConsumptionRate = 10,
            staminaRegenerationRate = 5,
            Level = 1,
            playerSpeed = 2,
            sprintMod = 2,
            jumpMax = 1,
            jumpHeight = 4,
            gravityValue = -15,
            currentQuestIndex = 0, // Start with the first quest by default
            currentQuestStepIndex = 0

        };
    }
}
