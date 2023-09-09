using UnityEngine;
using System.Collections.Generic;

public class ItemFactory : MonoBehaviour
{
    public static ItemFactory Instance { get; private set; }

    [SerializeField] List<ItemEntry> itemEntries;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [System.Serializable]
    public class ItemEntry
    {
        public BaseItemStats item;
        [Range(0, 1)] public float dropChance;
    }


    public BaseItemStats CreateItem(string itemName)
    {
        ItemEntry foundEntry = itemEntries.Find(entry => entry.item.itemName == itemName);

        if (foundEntry != null)
        {
            // Here, you could have further setup logic for the item if needed.
            return foundEntry.item;
        }
        else
        {
            Debug.LogError($"Item with the name {itemName} not found.");
            return null;
        }
    }

    // This function will provide a random item for the lootbox
    public BaseItemStats CreateRandomItem()
    {
        if (itemEntries.Count == 0)
        {
            Debug.LogError("No items available in the factory.");
            return null;
        }

        List<BaseItemStats> weightedItemList = new List<BaseItemStats>();

        foreach (var entry in itemEntries)
        {
            int weight = (int)(entry.dropChance * 100); // converting float probability to an integer weight
            for (int i = 0; i < weight; i++)
            {
                weightedItemList.Add(entry.item);
            }
        }

        if (weightedItemList.Count == 0)
        {
            Debug.LogError("No items passed the drop chance check.");
            return null;
        }

        int randomIndex = Random.Range(0, weightedItemList.Count);
        return weightedItemList[randomIndex];
    }
}