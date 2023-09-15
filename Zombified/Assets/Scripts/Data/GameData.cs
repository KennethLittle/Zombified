using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public PlayerData playerData;
    public List<EnemyData> enemiesData;

    // Constructor that accepts PlayerData and List<EnemyData>
    public GameData(PlayerData pD, List<EnemyData> eD)
    {
        playerData = pD;
        enemiesData = eD;
    }
}