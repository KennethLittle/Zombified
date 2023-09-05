using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory Item", menuName = "Inventory/Item")]
public class Inventoryitem : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    public int itemSize;
}
