using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PopulateItems", menuName = "Inventory System/PopulateItems")]
public class PopulateItems : ScriptableObject
{
    [SerializeField] private ItemManager itemManager;

    public BaseItemStats GetItemStats(string itemName)
    {
        return itemManager.GetItem<BaseItemStats>(itemName);
    }
}
