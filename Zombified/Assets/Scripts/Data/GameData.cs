using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public PlayerData playerData;
    public List<EnemyData> enemiesData;
    public List<QuestSaveData> questsSaveData;


    // Constructor that accepts PlayerData and List<EnemyData>
    public GameData(PlayerData pD, List<EnemyData> eD, List<QuestSaveData> qSD)
    {
        playerData = pD;
        enemiesData = eD;
        questsSaveData = qSD;
    }
}