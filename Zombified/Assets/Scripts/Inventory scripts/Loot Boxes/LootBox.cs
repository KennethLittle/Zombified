using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LootBox : MonoBehaviour
{
    public List<ScriptableObject> LootList = new List<ScriptableObject>();

    private void Start()
    {

    }
    List<ScriptableObject> droppedLoot()
    {
        int random = Random.Range(0, 101);
        List<ScriptableObject> droppingLoot = new List<ScriptableObject>();

        foreach (ScriptableObject loot in LootList)
        {
            if (random <= 15)
            {

            }

        }
        return droppingLoot;
    }
}
