using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

[System.Serializable]
public class InventoryItem
{
    public string itemName;                                     //itemName of the item
    public int itemID;                                          //itemID of the item
    public string itemDescription;                                     //itemDesc of the item
    public Sprite itemIcon;                                     //itemIcon of the item
    public GameObject itemModel;                                //itemModel of the item
    public int itemValue = 1;                                   //itemValue is at start 1
    public ItemType itemType;                                   //itemType of the Item
    public float itemWeight;                                    //itemWeight of the item
    public int maxStack = 1;
    public int indexItemInList = 999;
    public int rarity;
    public GameObject projectilePrefab;  // Prefab for the projectile (e.g. bullet, arrow, etc.)
    public float fireRate; // Number of times weapon can be fired in a second
    public float range; // The range of the weapon if it's melee
    public WeaponDetails weaponDetails;


    [SerializeField]
    public List<ItemStats> itemStats = new List<ItemStats>();

    public InventoryItem() {}

    public InventoryItem(string name, int id, string description, Sprite icon, GameObject model, int maxStack, ItemType type, string sendmessagetext, List<ItemStats> itemStats, float dist, float firing, GameObject project)                 //function to create a instance of the Item
    {
        itemName = name;
        itemID = id;
        itemDescription = description;
        itemIcon = icon;
        itemModel = model;
        itemType = type;
        this.maxStack = maxStack;
        this.itemStats = itemStats;
        if(itemType == ItemType.Weapon)
        {
            fireRate = firing;
            range = dist;
            project = projectilePrefab;           
        }
    }

    public InventoryItem getCopy()
    {
        return (InventoryItem)this.MemberwiseClone();
    }
}
