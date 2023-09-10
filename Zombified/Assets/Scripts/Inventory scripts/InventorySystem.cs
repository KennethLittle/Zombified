using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance;

    public int inventorybag = 32;
    public List<BaseItemStats> items = new List<BaseItemStats>();

    private BaseItemStats primaryWeapon;
    private BaseItemStats secondaryWeapon;
    public Transform primaryWeaponSlot;
    public Transform secondaryWeaponSlot;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool AddItem(BaseItemStats item)
    {
        if (items.Count < inventorybag)
        {
            items.Add(item);

            // Check if this is the first primary weapon and if the primary slot is empty.
            if (item.itemType == ItemType.PrimaryWeapon && primaryWeapon == null)
            {
                EquipPrimaryWeapon(item);
            }
            // Similarly, you can add for secondary weapon if you wish to keep a separate slot.

            return true;
        }
        return false;
    }

    public void RemoveItem(BaseItemStats item)
    {
        items.Remove(item);
    }

    public void EquipPrimaryWeapon(BaseItemStats weapon)
    {
        if (weapon.itemType == ItemType.PrimaryWeapon && primaryWeapon == null)
        {
            primaryWeapon = weapon;
            RemoveItem(weapon);  // Remove weapon from inventory once equipped

            GameObject equippedWeapon = Instantiate(weapon.modelPrefab, primaryWeaponSlot.transform.position, Quaternion.identity, primaryWeaponSlot.transform);

            // You can check and print out whether the MeshFilter and MeshRenderer exist on the instantiated weapon:
            if (equippedWeapon.GetComponent<MeshFilter>() && equippedWeapon.GetComponent<MeshRenderer>())
            {
                Debug.Log("Weapon has MeshFilter and MeshRenderer");
            }
            else
            {
                Debug.LogWarning("Weapon might be missing MeshFilter or MeshRenderer");
            }
        }
    }

    public void EquipSecondaryWeapon(BaseItemStats weapon)
    {
        if (weapon.itemType == ItemType.SecondaryWeapon)
        {
            secondaryWeapon = weapon;
            RemoveItem(weapon);  // Remove weapon from inventory once equipped
        }
    }

    public void UseItem(BaseItemStats item)
    {
        switch (item.itemType)
        {
            case ItemType.MedPack:
                // Apply med pack effect, for example, heal the player
                RemoveItem(item);
                break;

            case ItemType.AmmoBox:
                // Refill the weapon's ammo
                RemoveItem(item);
                break;

            // ... Any other usable item types can be added here

            default:
                // Do nothing for other item types
                break;
        }
    }
}
