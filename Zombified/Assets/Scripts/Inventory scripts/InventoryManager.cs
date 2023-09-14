using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public int inventorybag = 32;
    public List<InventoryItem> items = new List<InventoryItem>();
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
    public bool AddItem(InventoryItem item)
    {
        if(items.Count < inventorybag)
        {
            items.Add(item);
            return true;
        }
        return false;
    }

    public void RemoveItem(InventoryItem item)
    {
        items.Remove(item);
    }
}
