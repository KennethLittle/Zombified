using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LootBoxUI : MonoBehaviour
{

    public InventoryUI inventoryUI;
    public static LootBoxUI Instance;
    public Button lootAllButton;

    [System.Serializable]
    public struct LootItem
    {
        public BaseItemStats item;
        [Range(0, 1)] public float dropChance;
    }

    public Transform storage; // A UI container (e.g., a Grid) to hold item UI representations.
    public GameObject itemUIPrefab; // A prefab representing an individual item in the UI.
    public Transform[] slots;
    public List<LootItem> possibleLootItems;

    private bool isStorageClosed = true;
    private bool lootDropped = false;
    private List<BaseItemStats> currentLoot = new List<BaseItemStats>();

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
        currentLoot = DropLoot();
        Debug.Log("Loot assigned to currentLoot with count: " + currentLoot.Count);
        DisplayLoot();
        lootAllButton.onClick.AddListener(LootAllItems);
    }

    List<BaseItemStats> DropLoot()
    {
        List<BaseItemStats> loot = new List<BaseItemStats>();
        int numberOfItemsToDrop = Random.Range(1, 9); // This will give a number between 1 and 8 inclusive.

        for (int i = 0; i < numberOfItemsToDrop; i++)
        {
            BaseItemStats potentialDrop = ItemFactory.Instance.CreateRandomItem();

            if (potentialDrop && possibleLootItems.Any(li => li.item == potentialDrop))
            {
                loot.Add(potentialDrop);
                Debug.Log($"{potentialDrop.itemName} added to loot");
            }
        }

        Debug.Log("Current loot list count after item addition: " + loot.Count);
        lootDropped = true;

        return loot;
    }

    public void ToggleUI()
    {
        Debug.Log("ToggleUI function called.");
        if (isStorageClosed)
        {
            Cursor.lockState = CursorLockMode.None; // This unlocks the cursor
            Cursor.visible = true; // This makes the cursor visible
            DisplayLoot();
            storage.gameObject.SetActive(true);
            isStorageClosed = false;

            // Open the Inventory
            inventoryUI.OpenInventoryDirectly();

        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked; // This locks the cursor to the center
            Cursor.visible = false; // This hides the cursor
            storage.gameObject.SetActive(false);
            isStorageClosed = true;
            gameManager.instance.inventory.SetActive(false);
        }
    }

    private void DisplayLoot()
    {
        if (!lootDropped) return;

        Debug.Log("Entering DisplayLoot method.");
        Debug.Log("DisplayLoot called. CurrentLoot count: " + currentLoot.Count);

        DebugPrintChildHierarchy(storage);
        // Clear previous displayed items
        foreach (Transform slot in slots)
        {
            DestroyAllChildren(slot);
        }

        int slotIndex = 0;
        foreach (var item in currentLoot)
        {
            if (slotIndex < slots.Length)
            {
                GameObject itemUIObject = Instantiate(itemUIPrefab, slots[slotIndex]);
                SetIcon(itemUIObject, item.icon);
                slotIndex++; // Move to the next slot for the next item
            }
            else
            {
                Debug.LogWarning("No more slots available to display loot.");
                break; // Break out of the loop once we've run out of slots
            }
        }
        DebugPrintChildHierarchy(storage);
        lootDropped = false;
    }

    public void LootAllItems()
    {
        foreach (var item in currentLoot)
        {
            // Move the item to the inventory
            inventoryUI.AddItemToInventory(item);
            InventorySystem.Instance.AddItem(item);
        }
        currentLoot.Clear(); // Clear the loot

        // Clear item UI representations from the loot box slots
        foreach (Transform slot in slots)
        {
            DestroyAllChildren(slot);
        }

        DisplayLoot(); // This will essentially clear the UI display since currentLoot is now empty
    }

    private void SetIcon(GameObject itemUIObject, Sprite iconSprite)
    {
        Image imageComponent = itemUIObject.GetComponent<Image>();
        if (!imageComponent) // if no Image component on the main object, search its children
        {
            imageComponent = itemUIObject.GetComponentInChildren<Image>();
        }

        if (imageComponent)
        {
            Debug.Log("Setting icon for: " + itemUIObject.name);
            imageComponent.sprite = iconSprite;
            Debug.Log("Icon sprite for " + itemUIObject.name + " set to: " + iconSprite.name);
        }
        else
        {
            Debug.LogError("No Image component found for: " + itemUIObject.name);
        }
    }

    public bool MoveItemToInventory(BaseItemStats item)
    {
        // Try adding the item to the InventorySystem
        if (InventorySystem.Instance.AddItem(item))
        {
            // If successfully added to InventorySystem, add to UI
            inventoryUI.AddItemToInventory(item);
            InventorySystem.Instance.AddItem(item);

            // Remove the item from currentLoot
            currentLoot.Remove(item);
            DisplayLoot(); // Redisplay loot without the moved item

            return true; // Indicate success
        }

        // If reached here, the item was not added to the inventory
        return false;
    }

    public void RemoveItemFromLoot(BaseItemStats item)
    {
        if (currentLoot.Contains(item))
        {
            currentLoot.Remove(item);
            DisplayLoot();
        }
        else
        {
            Debug.LogWarning("Trying to remove an item that isn't in the currentLoot list.");
        }
    }

    private void DestroyAllChildren(Transform parentTransform)
    {
        for (int i = parentTransform.childCount - 1; i >= 0; i--)
        {
            Transform child = parentTransform.GetChild(i);
            Destroy(child.gameObject);
        }
    }

    private void DebugPrintChildHierarchy(Transform parentTransform)
    {
        Debug.Log("Children of: " + parentTransform.name);
        foreach (Transform child in parentTransform)
        {
            Debug.Log("--> " + child.name);
            foreach (Transform subChild in child)
            {
                Debug.Log("----> " + subChild.name);
            }
        }
    }

    private void DestroyLootBoxPrefab()
    {
        if (this.gameObject.CompareTag("LootBox"))
        {
            Destroy(this.gameObject);
        }
    }
}

