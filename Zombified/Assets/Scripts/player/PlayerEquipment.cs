using Palmmedia.ReportGenerator.Core;
using System.Diagnostics;
using System.Net;
using System.Numerics;
using System.Security.Principal;
using System;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using Unity.VisualScripting;
using UnityEditor.Presets;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;


public class PlayerEquipment : MonoBehaviour
{
    private InventorySystem inventory;
    public static PlayerEquipment Instance;
    private PlayerStat playerstat;
    public GameObject inventorysetup;
    public GameObject charactersetup;
    public GameObject craftSystem;
    private InventorySystem craftInventory;
    private CraftSystem craft;
    private InventorySystem mainInventory;
    private InventorySystem characterInventory;
    private HelperTool Toolbar;
    private WeaponDetails weaponDetails;
    private GameStateManager stateManager;
    private InputManager inputManagerData;
    public InventoryItem equippedWeapon;
    private Transform firePoint;
    public GameObject HPMANACanvas;

    Image hpImage;
    Image staminaImage;

    float maxHealth = 100;
    float stamina = 100;
    float maxDamage = 0;
    float maxArmor = 0;

    public float currentHealth = 100;
    float currentStamina = 100;
    float currentDamage = 0;
    float currentArmor = 0;

    int normalSize = 3;

    public void OnEnable()
    {
        InventorySystem.ItemEquip += OnBackpack;
        InventorySystem.UnEquipItem += UnEquipBackpack;
        InventorySystem.ItemEquip += OnGearItem;
        InventorySystem.ItemConsumed += OnConsumeItem;
        InventorySystem.UnEquipItem += OnUnEquipItem;
        InventorySystem.ItemEquip += EquipWeapon;
        InventorySystem.UnEquipItem += UnEquipWeapon;
    }

    public void OnDisable()
    {
        InventorySystem.ItemEquip -= OnBackpack;
        InventorySystem.UnEquipItem -= UnEquipBackpack;

        InventorySystem.ItemEquip -= OnGearItem;
        InventorySystem.ItemConsumed -= OnConsumeItem;
        InventorySystem.UnEquipItem -= OnUnEquipItem;

        InventorySystem.UnEquipItem -= UnEquipWeapon;
        InventorySystem.ItemEquip -= EquipWeapon;
    }

    public void EquipWeapon(InventoryItem weaponItem)
    {
        equippedWeapon = weaponItem;

        equippedWeapon.weaponDetails = weaponItem.weaponDetails;

    }


    public void UnEquipWeapon(InventoryItem item)
    {
        equippedWeapon = null;
    }

    public void UseItem(InventoryItem item)
    {
        switch (item.itemType)
        {
            case ItemType.AmmoBox:
                UseAmmo(item);
                break;
            case ItemType.MedPack:
                UseMedpack(item);
                break;
        }

    }

    public void UseAmmo(InventoryItem ammoItem)
    {
        if (equippedWeapon != null) // Assumes that your weapon uses ammo
        {
            equippedWeapon.weaponDetails.ammoAdditional += ammoItem.ammoDetails.ammo;
            PlayerManager.instance.playerScript.updatePlayerUI();
        }
        // Remove the ammo item from inventory after use
        inventory.deleteItemFromInventoryWithGameObject(ammoItem);
    }

    public void UseMedpack(InventoryItem medpackItem)
    {
        PlayerStat.Instance.HP += medpackItem.medpackDetails.healingValue;
        // Ensure player's HP doesn't exceed maxHealth
        if (PlayerStat.Instance.HP > PlayerStat.Instance.HPMax)
        {
            PlayerStat.Instance.HP = PlayerStat.Instance.HPMax;
            PlayerManager.instance.playerScript.updatePlayerUI();

        }

        // Remove the medpack from inventory after use
        inventory.deleteItemFromInventoryWithGameObject(medpackItem);
    }

    void OnBackpack(InventoryItem item)
    {
        if (item.itemType == ItemType.Backpack)
        {
            for (int i = 0; i < item.itemStats.Count; i++)
            {
                if (mainInventory == null)
                    mainInventory = inventorysetup.GetComponent<InventorySystem>();
                mainInventory.sortItems();
                if (item.itemStats[i].attributeName == "Slots")
                    changeInventorySize(item.itemStats[i].attributeValue);
            }
        }
    }

    void UnEquipBackpack(InventoryItem item)
    {
        if (item.itemType == ItemType.Backpack)
            changeInventorySize(normalSize);
    }

    void changeInventorySize(int size)
    {
        dropTheRestItems(size);

        if (mainInventory == null)
            mainInventory = inventorysetup.GetComponent<InventorySystem>();
        if (size == 3)
        {
            mainInventory.width = 3;
            mainInventory.height = 1;
            mainInventory.updateSlotAmount();
            mainInventory.adjustInventorySize();
        }
        if (size == 6)
        {
            mainInventory.width = 3;
            mainInventory.height = 2;
            mainInventory.updateSlotAmount();
            mainInventory.adjustInventorySize();
        }
        else if (size == 12)
        {
            mainInventory.width = 4;
            mainInventory.height = 3;
            mainInventory.updateSlotAmount();
            mainInventory.adjustInventorySize();
        }
        else if (size == 16)
        {
            mainInventory.width = 4;
            mainInventory.height = 4;
            mainInventory.updateSlotAmount();
            mainInventory.adjustInventorySize();
        }
        else if (size == 24)
        {
            mainInventory.width = 6;
            mainInventory.height = 4;
            mainInventory.updateSlotAmount();
            mainInventory.adjustInventorySize();
        }
    }

    void dropTheRestItems(int size)
    {
        if (size < mainInventory.ItemsInInventory.Count)
        {
            for (int i = size; i < mainInventory.ItemsInInventory.Count; i++)
            {
                GameObject dropItem = (GameObject)Instantiate(mainInventory.ItemsInInventory[i].itemModel);
                dropItem.AddComponent<ItemPickup>();
                dropItem.GetComponent<ItemPickup>().item = mainInventory.ItemsInInventory[i];
                dropItem.transform.localPosition = GameObject.FindGameObjectWithTag("Player").transform.localPosition;
            }
        }
    }

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }


        if (inventorysetup != null)
            mainInventory = inventorysetup.GetComponent<InventorySystem>();
        if (charactersetup != null)
            craftInventory = charactersetup.GetComponent<InventorySystem>();
    }



    public void OnConsumeItem(InventoryItem item)
    {
        for (int i = 0; i < item.itemStats.Count; i++)
        {
            if (item.itemStats[i].attributeName == "Health")
            {
                if ((currentHealth + item.itemStats[i].attributeValue) > maxHealth)
                    currentHealth = maxHealth;
                else
                    currentHealth += item.itemStats[i].attributeValue;
            }
            if (item.itemStats[i].attributeName == "Mana")
            {
                if ((currentStamina + item.itemStats[i].attributeValue) > stamina)
                    currentStamina = stamina;
                else
                    currentStamina += item.itemStats[i].attributeValue;
            }
            if (item.itemStats[i].attributeName == "Armor")
            {
                if ((currentArmor + item.itemStats[i].attributeValue) > maxArmor)
                    currentArmor = maxArmor;
                else
                    currentArmor += item.itemStats[i].attributeValue;
            }
            if (item.itemStats[i].attributeName == "Damage")
            {
                if ((currentDamage + item.itemStats[i].attributeValue) > maxDamage)
                    currentDamage = maxDamage;
                else
                    currentDamage += item.itemStats[i].attributeValue;
            }
        }
       
    }

    public void OnGearItem(InventoryItem item)
    {
        for (int i = 0; i < item.itemStats.Count; i++)
        {
            if (item.itemStats[i].attributeName == "Health")
                maxHealth += item.itemStats[i].attributeValue;
            if (item.itemStats[i].attributeName == "Mana")
                stamina += item.itemStats[i].attributeValue;
            if (item.itemStats[i].attributeName == "Armor")
                maxArmor += item.itemStats[i].attributeValue;
            if (item.itemStats[i].attributeName == "Damage")
                maxDamage += item.itemStats[i].attributeValue;
        }
        
    }

    public void OnUnEquipItem(InventoryItem item)
    {
        for (int i = 0; i < item.itemStats.Count; i++)
        {
            if (item.itemStats[i].attributeName == "Health")
                maxHealth -= item.itemStats[i].attributeValue;
            if (item.itemStats[i].attributeName == "Mana")
                stamina -= item.itemStats[i].attributeValue;
            if (item.itemStats[i].attributeName == "Armor")
                maxArmor -= item.itemStats[i].attributeValue;
            if (item.itemStats[i].attributeName == "Damage")
                maxDamage -= item.itemStats[i].attributeValue;
        }
        
    }

    void ToggleSystem()
    {
        if (!charactersetup.activeSelf)
        {
            craftInventory.openInventory();
            charactersetup.SetActive(true);
            GameStateManager.instance.ChangeState(GameStateManager.GameState.Paused);
        }
        else
        {
            craftInventory.closeInventory();
            charactersetup.SetActive(false);
            GameStateManager.instance.ChangeState(GameStateManager.GameState.Playing);
        }
    }

    void ToggleInventory()
    {
        if (!inventorysetup.activeSelf)
        {
            mainInventory.openInventory();
            inventorysetup.SetActive(true);
            GameStateManager.instance.ChangeState(GameStateManager.GameState.Paused);
        }
        else
        {
            mainInventory.closeInventory();
            inventorysetup.SetActive(false);
            GameStateManager.instance.ChangeState(GameStateManager.GameState.Playing);
        }
    }







    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            ToggleSystem();
            ToggleInventory();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
           ToggleInventory();
            ToggleSystem();
        }

    }
}

