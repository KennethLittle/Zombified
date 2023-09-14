using UnityEngine;

[System.Serializable]
public class EnemyStat : MonoBehaviour
{
    public int baseDamage;
    public int baseHP;
    public int baseDefense;
    public int currentHP;
    private PlayerStat playerStat;



    // Assuming these multipliers will determine the rate at which the stats grow per level.
    // You can adjust these to your liking.
    [Range(0.1f, 5f)] public float damageMultiplier = 1.1f;
    [Range(0.1f, 5f)] public float hpMultiplier = 1.2f;
    [Range(0.1f, 5f)] public float defenseMultiplier = 1.1f;

    private int playerLevel;

    // Constructor
    public EnemyStat(int playerLevel)
    {
        this.playerLevel = playerLevel;
    }

    public int CurrentDamage
    {
        get
        {
            return Mathf.RoundToInt(baseDamage * Mathf.Pow(damageMultiplier, playerLevel - 1));
        }
    }

    public int CurrentHP
    {
        get { return currentHP; }
        set
        {
            currentHP = value;
            if (currentHP < 0) currentHP = 0; // Ensure HP never goes below 0
        }
    }

    public int CurrentDefense
    {
        get
        {
            return Mathf.RoundToInt(baseDefense * Mathf.Pow(defenseMultiplier, playerLevel - 1));
        }
    }

    // If you need to update the player's level on the fly
    public void UpdatePlayerLevel(int newLevel)
    {
        this.playerLevel = newLevel;
    }

    public int Level
    {
        get
        {
            return playerStat.Level;
        }
    }
    public int baseXP = 10;

    public int CalculateExperienceReward()
    {
        return Mathf.RoundToInt(baseXP * Mathf.Pow(1 + (float)Level / 10, 2));
    }
}
