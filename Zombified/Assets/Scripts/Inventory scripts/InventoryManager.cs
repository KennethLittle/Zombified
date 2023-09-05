using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public int inventorybag = 32;
    public List<Inventoryitem> items = new List<Inventoryitem>();
    // Start is called before the first frame update
    void Awake()
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

    // Update is called once per frame
    public bool AddItem(Inventoryitem item)
    {
        if(items.Count < inventorybag)
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
