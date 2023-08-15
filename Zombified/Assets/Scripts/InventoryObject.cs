using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory Object", menuName = "Inventory/Object")]
public class InventoryObject : ScriptableObject
{
    public string WeaponName;
    public Sprite WeaponIcon;
    public int WeaponSize;
}
