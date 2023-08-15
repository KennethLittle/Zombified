using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySetup : MonoBehaviour
{
    public static InventorySetup Instance;
    public int inventorybag = 32;
    public GameObject PhysicalWeapon;
    public List<WeaponStats> Weapons = new List<WeaponStats>();
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(PhysicalWeapon);
        }
    }

    // Update is called once per frame
    public bool AddItem(WeaponStats item)
    {
        if(Weapons.Count < inventorybag)
        {
            Weapons.Add(item);
            return true;
        }
        return false;
    }

    public void RemoveItem(WeaponStats item)
    {
        Weapons.Remove(item);
    }
}
