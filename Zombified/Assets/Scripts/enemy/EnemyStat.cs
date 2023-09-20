using UnityEngine;

[System.Serializable]
public class EnemyStat : MonoBehaviour
{
    public int enemyLevel;
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



    public int CurrentDamage
    {
        get
        {
            return Mathf.RoundToInt(baseDamage * Mathf.Pow(damageMultiplier, PlayerStat.Instance.Level - 1));
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
            return Mathf.RoundToInt(baseDefense * Mathf.Pow(defenseMultiplier, PlayerStat.Instance.Level - 1));
        }
    }

    // If you need to update the player's level on the fly
    public void UpdateEnemyLevelBasedOnPlayer()
    {
        
        enemyLevel = Mathf.Max(1, PlayerStat.Instance.Level + 2); // The enemy will be at most 2 levels below the player
    }

    public int baseXP = 10;

    public int CalculateExperienceReward()
    {
        // This is a simplistic formula and might need tuning based on your game's balance
        return baseXP * (1 + enemyLevel / 10);
    }

    public EnemyData ExtractData()
    {
        EnemyData data = new EnemyData();

        enemyAI aiComponent = GetComponent<enemyAI>();
        if (aiComponent != null)
        {
            data.enemyID = aiComponent.enemyID;
        }

        data.position = transform.position;
        data.currentLevel = this.enemyLevel;
        data.currentHP = this.currentHP;
        data.baseDamage = this.baseDamage;
        data.baseDefense = this.baseDefense;
      

        return data;
    }
}
