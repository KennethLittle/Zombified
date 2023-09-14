using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Backpack,
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
}
