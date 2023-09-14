using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBaseForItems : MonoBehaviour
{             //The scriptableObject where the Item getting stored which you create(BataBaseForItems)

    [SerializeField]
    public List<InventoryItem> itemList = new List<InventoryItem>();              //List of items

    public InventoryItem getItemByID(int id)
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i].itemID == id)
                return itemList[i].getCopy();
        }
        return null;
    }

    public InventoryItem getItemByName(string name)
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i].itemName.ToLower().Equals(name.ToLower()))
                return itemList[i].getCopy();
        }
        return null;
    }
}
