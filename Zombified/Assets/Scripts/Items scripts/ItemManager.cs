using UnityEngine;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;

[CreateAssetMenu(fileName = "ItemManager", menuName = "Items")]
public class ItemManager : ScriptableObject
{
    public List<ScriptableObject> items; // This will store all your items

    // Here's a method to get a specific item by its name/type
    public T GetItem<T>(string name) where T : ScriptableObject
    {
        foreach (var item in items)
        {
            if (item.name == name && item is T)
            {
                return (T)item;
            }
        }
        return null;
    }

    // And other methods you might find useful...
}
