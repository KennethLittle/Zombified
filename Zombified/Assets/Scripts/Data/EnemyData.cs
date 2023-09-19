using UnityEngine;

[System.Serializable]
public class EnemyData
{
    public int enemyID;
    public Vector3 position;
    public int currentHP;
    public int baseDamage;
    public int baseDefense;
    public int currentLevel;
    // You can add more properties as needed, such as enemy type, level, etc.

    // Optional: If you want constructors or methods within this class, 
    // you can add them here. Currently, it's a simple data container.
}
