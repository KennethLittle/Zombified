using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance;

    public int inventorybag = 32;
    public List<BaseItemStats> items = new List<BaseItemStats>();

    private BaseItemStats equippedWeapon;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
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

            // Equip the first weapon if none is equipped yet.
            if (item.itemType == ItemType.Weapon && equippedWeapon == null)
            {
                EquipWeapon(item);
            }
            return true;
        }
        return false;
    }

    public void EquipWeapon(BaseItemStats weapon)
    {
        if (weapon.itemType == ItemType.Weapon)
        {
            equippedWeapon = weapon;
            RemoveItem(weapon);  // Remove weapon from inventory once equipped

            // Notify the player script to equip the weapon visually
            
            PlayerManager.instance.playerScript.EquipWeapon(weapon);

            // Add the weapon to the equipped weapons list in playerController
            PlayerManager.instance.playerScript.equippedWeapons.Add(weapon.modelPrefab);
            PlayerManager.instance.playerScript.currentWeaponIndex = PlayerManager.instance.playerScript.equippedWeapons.Count - 1; // Update index to the last weapon (most recent one equipped)
        }
    }

    public void RemoveItem(BaseItemStats item)
    {
        items.Remove(item);
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
