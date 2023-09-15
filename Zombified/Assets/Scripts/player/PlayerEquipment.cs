using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;

public class PlayerEquipment : MonoBehaviour
{
    public GameObject inventorysetup;
    public GameObject charactersetup;
    public GameObject craftSystem;
    private InventorySystem craftInventory;
    private CraftSystem craft;
    private InventorySystem mainInventory;
    private InventorySystem characterInventory;
    private HelperTool Toolbar;

    private InputManager inputManagerData;

    public GameObject HPMANACanvas;

    Text hpText;
    Text manaText;
    Image hpImage;
    Image manaImage;

    float maxHealth = 100;
    float maxMana = 100;
    float maxDamage = 0;
    float maxArmor = 0;

    public float currentHealth = 60;
    float currentMana = 100;
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

    void EquipWeapon(InventoryItem item)
    {
        if (item.itemType == ItemType.Weapon)
        {
            //add the weapon if you unequip the weapon
        }
    }

    void UnEquipWeapon(InventoryItem item)
    {
        if (item.itemType == ItemType.Weapon)
        {
            //delete the weapon if you unequip the weapon
        }
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
        //if (HPMANACanvas != null)
        //{
        //    hpText = HPMANACanvas.transform.GetChild(1).GetChild(0).GetComponent<Text>();

        //    manaText = HPMANACanvas.transform.GetChild(2).GetChild(0).GetComponent<Text>();

        //    hpImage = HPMANACanvas.transform.GetChild(1).GetComponent<Image>();
        //    manaImage = HPMANACanvas.transform.GetChild(1).GetComponent<Image>();

        //    UpdateHPBar();
        //    UpdateManaBar();
        //}

        if (inputManagerData == null)
            inputManagerData = (InputManager)Resources.Load("InputManager");

        if (craftSystem != null)
            craft = craftSystem.GetComponent<CraftSystem>();

        if (GameObject.FindGameObjectWithTag("Tooltip") != null)
            Toolbar = GameObject.FindGameObjectWithTag("Tooltip").GetComponent<HelperTool>();
        if (inventorysetup != null)
            mainInventory = inventorysetup.GetComponent<InventorySystem>();
        if (charactersetup != null)
            craftInventory = charactersetup.GetComponent<InventorySystem>();
        if (craftSystem != null)
            craftInventory = craftSystem.GetComponent<InventorySystem>();
    }

    //void UpdateHPBar()
    //{
    //    hpText.text = (currentHealth + "/" + maxHealth);
    //    float fillAmount = currentHealth / maxHealth;
    //    hpImage.fillAmount = fillAmount;
    //}

    //void UpdateManaBar()
    //{
    //    manaText.text = (currentMana + "/" + maxMana);
    //    float fillAmount = currentMana / maxMana;
    //    manaImage.fillAmount = fillAmount;
    //}


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
                if ((currentMana + item.itemStats[i].attributeValue) > maxMana)
                    currentMana = maxMana;
                else
                    currentMana += item.itemStats[i].attributeValue;
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
        //if (HPMANACanvas != null)
        //{
        //    UpdateManaBar();
        //    UpdateHPBar();
        //}
    }

    public void OnGearItem(InventoryItem item)
    {
        for (int i = 0; i < item.itemStats.Count; i++)
        {
            if (item.itemStats[i].attributeName == "Health")
                maxHealth += item.itemStats[i].attributeValue;
            if (item.itemStats[i].attributeName == "Mana")
                maxMana += item.itemStats[i].attributeValue;
            if (item.itemStats[i].attributeName == "Armor")
                maxArmor += item.itemStats[i].attributeValue;
            if (item.itemStats[i].attributeName == "Damage")
                maxDamage += item.itemStats[i].attributeValue;
        }
        //if (HPMANACanvas != null)
        //{
        //    UpdateManaBar();
        //    UpdateHPBar();
        //}
    }

    public void OnUnEquipItem(InventoryItem item)
    {
        for (int i = 0; i < item.itemStats.Count; i++)
        {
            if (item.itemStats[i].attributeName == "Health")
                maxHealth -= item.itemStats[i].attributeValue;
            if (item.itemStats[i].attributeName == "Mana")
                maxMana -= item.itemStats[i].attributeValue;
            if (item.itemStats[i].attributeName == "Armor")
                maxArmor -= item.itemStats[i].attributeValue;
            if (item.itemStats[i].attributeName == "Damage")
                maxDamage -= item.itemStats[i].attributeValue;
        }
        //if (HPMANACanvas != null)
        //{
        //    UpdateManaBar();
        //    UpdateHPBar();
        //}
    }



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(inputManagerData.CharacterSystemKeyCode))
        {
            if (!charactersetup.activeSelf)
            {
                craftInventory.openInventory();
            }
            else
            {
                if (Toolbar != null)
                    Toolbar.deactivateTooltip();
                craftInventory.closeInventory();
            }
        }

        if (Input.GetKeyDown(inputManagerData.InventoryKeyCode))
        {
            if (!inventorysetup.activeSelf)
            {
                mainInventory.openInventory();
            }
            else
            {
                if (Toolbar != null)
                    Toolbar.deactivateTooltip();
                mainInventory.closeInventory();
            }
        }

        if (Input.GetKeyDown(inputManagerData.CraftSystemKeyCode))
        {
            if (!craftSystem.activeSelf)
                craftInventory.openInventory();
            else
            {
                if (craft != null)
                    craft.backToInventory();
                if (Toolbar != null)
                    Toolbar.deactivateTooltip();
                craftInventory.closeInventory();
            }
        }

    }

}

