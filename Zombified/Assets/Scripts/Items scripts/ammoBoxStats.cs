using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ammo Box Object", menuName = "Inventory System/Items/Ammo Box")]
public class ammoBoxStats : ScriptableObject
{
    public int ammoAmount;
    public GameObject model;
    public Sprite icon;
}
