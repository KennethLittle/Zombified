using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBaseForItems : MonoBehaviour
{             //The scriptableObject where the Item getting stored which you create(BataBaseForItems)

    [SerializeField]
    public List<InventoryItem> itemNumber = new List<InventoryItem>();              //List of items

    public InventoryItem getItemByID(int id)
    {
        for (int i = 0; i < itemNumber.Count; i++)
        {
            if (itemNumber[i].itemID == id)
                return itemNumber[i].getCopy();
        }
        return null;
    }

    public InventoryItem getItemByName(string name)
    {
        for (int i = 0; i < itemNumber.Count; i++)
        {
            if (itemNumber[i].itemName.ToLower().Equals(name.ToLower()))
                return itemNumber[i].getCopy();
        }
        return null;
    }
}
