using UnityEngine;
using System.Collections.Generic;

public class ItemFactory : MonoBehaviour
{
    public static ItemFactory Instance { get; private set; }

    [SerializeField]
    private List<BaseItemStats> items;

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

    public BaseItemStats CreateItem(string itemName)
    {
        BaseItemStats item = items.Find(i => i.name == itemName);
        if (item != null)
        {
            // Here, you could have further setup logic for the item if needed.
            return item;
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
        if (items.Count == 0)
        {
            Debug.LogError("No items available in the factory.");
            return null;
        }

        int randomIndex = Random.Range(0, items.Count);
        return items[randomIndex];
    }
}