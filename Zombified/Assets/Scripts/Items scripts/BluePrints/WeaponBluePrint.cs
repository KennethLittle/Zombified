using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBluePrint : MonoBehaviour
{
    public List<int> coins = new List<int>();
    public List<int> weapon = new List<int>();
    public InventoryItem weaponrarity;
    public int weaponLevel;
    
    public WeaponBluePrint(List<int> coins, int weaponLevel, List<int> weapon, InventoryItem Gun)
    {
        this.coins = coins;
        this.weapon = weapon;
        weaponrarity = Gun;
    }

}
