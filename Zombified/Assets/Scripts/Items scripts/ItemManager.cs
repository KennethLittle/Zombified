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

//Here's how you can set it up and use it:

//ItemManager Setup:

//Create an instance of ItemManager in your project (Right-click in your assets panel, navigate through "Items" and select "ItemManager").
//Drag your MedPack, Ammobox, and Weapon scriptable objects into the items list of the ItemManager.
//Usage:

//Whenever you want to access any of your items in a script, you'd first reference your ItemManager and then call the method to retrieve the item.
//Example:
//csharp
//Copy code
//public ItemManager itemManager;  // Assign this in the inspector

//private void SomeMethod()
//{
//    MedPack medPack = itemManager.GetItem<MedPack>("MedPack");
//     Now use the medPack as you wish
//}