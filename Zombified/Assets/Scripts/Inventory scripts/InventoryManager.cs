using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<ItemManager> items = new List<ItemManager>();

    // Start is called before the first frame update
    private void Awake()
    {
            Instance = this;
    }

    // Update is called once per frame
    public void AddItem(ItemManager item)
    {
        items.Add(item);
    }

    public void RemoveItem(ItemManager item)
    {
        items.Remove(item);
    }
}
