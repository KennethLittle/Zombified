using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LootBoxUI : MonoBehaviour
{
    public Transform storage; // A UI container (e.g., a Grid) to hold item UI representations.
    public GameObject itemUIPrefab; // A prefab representing an individual item in the UI.
    public Transform[] slots;

    private bool isStorageClosed = true;
    private bool lootDropped = false;
    private List<BaseItemStats> currentLoot = new List<BaseItemStats>();

    private void Awake()
    {
        currentLoot = DropLoot();
        Debug.Log("Loot assigned to currentLoot with count: " + currentLoot.Count);
    }

    BaseItemStats GetItemByName(string name)
    {
        return ItemFactory.Instance.CreateItem(name);
    }

    List<BaseItemStats> DropLoot()
    {
        List<BaseItemStats> loot = new List<BaseItemStats>();

        int amountDropped = UnityEngine.Random.Range(1, 5);
        Debug.Log("Amount of items to be dropped: " + amountDropped);

        for (int i = 0; i < amountDropped; i++)
        {
            BaseItemStats randomItem = ItemFactory.Instance.CreateRandomItem(); // Fetching random item
            if (randomItem != null)
            {
                loot.Add(randomItem);
                Debug.Log($"{randomItem.itemName} added to loot");
            }
        }

        foreach (var item in loot)
        {
            Debug.Log("Dropped item: " + item.itemName);
        }

        Debug.Log("Current loot list count after item addition: " + loot.Count);

        lootDropped = true;

        return loot;
    }

    public void ToggleUI()
    {
        if (isStorageClosed)
        {
            DisplayLoot();
            storage.gameObject.SetActive(true);
            isStorageClosed = false;
        }
        else
        {
            storage.gameObject.SetActive(false);
            isStorageClosed = true;
        }
    }

    private void DisplayLoot()
    {
        if (!lootDropped) return;

        Debug.Log("Entering DisplayLoot method.");
        Debug.Log("DisplayLoot called. CurrentLoot count: " + currentLoot.Count);

        // Clear previous displayed items
        //foreach (Transform slot in slots)
        {
            //foreach (Transform child in slot)
            {
               // Destroy(child.gameObject);
            }
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

        lootDropped = false;
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
        }
        else
        {
            Debug.LogError("No Image component found for: " + itemUIObject.name);
        }
    }
}

