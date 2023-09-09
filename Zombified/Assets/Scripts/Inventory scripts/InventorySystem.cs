using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance;

    public int inventorybag = 32;
    public List<Inventoryitem> items = new List<Inventoryitem>();

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

    public bool AddItem(Inventoryitem item)
    {
        if (items.Count > inventorybag)
        {
            items.Add(item);
            return true;
        }
        return false;
    }

    public void RemoveItem(Inventoryitem item)
    {
        items.Remove(item);
    }
}
