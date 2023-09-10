using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Weapon,
    MedPack,
    AmmoBox,
    AK,
    AssaultRifle,
    Shotgun,
    Sniper
    // Add more item types if needed
}


public abstract class BaseItemStats : ScriptableObject
{

    public string itemName = "New Item";
    public Sprite icon;
    public GameObject modelPrefab;
    public ItemType itemType;

    public static int GetCost(ItemType type)
    {
        switch (type)
        {
            default:
            case ItemType.AK: return 1200;
            case ItemType.AssaultRifle: return 1500;
            case ItemType.Shotgun: return 1000;
            case ItemType.Sniper: return 1800;
            case ItemType.AmmoBox: return 50;
            case ItemType.MedPack: return 50;
        }
    }

}
