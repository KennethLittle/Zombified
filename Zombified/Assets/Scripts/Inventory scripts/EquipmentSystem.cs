using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EquipmentSystem : MonoBehaviour
{
    [SerializeField]
    public int totalSlots;
    [SerializeField]
    public ItemType[] slotItemTypes = new ItemType[999];

    void Start()
    {
        ConsumingItems.equipmentSystem = GetComponent<EquipmentSystem>();
    }

    public void computeTotalSlots()
    {
        InventorySystem itemHolder = GetComponent<InventorySystem>();
        totalSlots = itemHolder.width * itemHolder.height;
    }

#if UNITY_EDITOR
    [MenuItem("Master System/Create/Equipment")] // creating the menu item
    public static void initializeEquipmentMenu()  // create the inventory at start
    {
        GameObject UIContainer = null;
        if (GameObject.FindGameObjectWithTag("Canvas") == null)
        {
            GameObject inventoryGroup = new GameObject();
            inventoryGroup.name = "Inventories";
            UIContainer = (GameObject)Instantiate(Resources.Load("Prefabs/Canvas - Inventory") as GameObject);
            UIContainer.transform.SetParent(inventoryGroup.transform, true);
            GameObject panelObject = (GameObject)Instantiate(Resources.Load("Prefabs/Panel - EquipmentManager") as GameObject);
            panelObject.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
            panelObject.transform.SetParent(UIContainer.transform, true);
            GameObject itemDrag = (GameObject)Instantiate(Resources.Load("Prefabs/DraggingItem") as GameObject);
            itemDrag.transform.SetParent(UIContainer.transform, true);
            Instantiate(Resources.Load("Prefabs/EventSystem") as GameObject);
            InventorySystem invInstance = panelObject.AddComponent<InventorySystem>();
           panelObject.AddComponent<InventoryDesign>();
            panelObject.AddComponent<EquipmentSystem>();
           invInstance.getPrefabs();
        }
        else
        {
            GameObject panelObject = (GameObject)Instantiate(Resources.Load("Prefabs/Panel - EquipmentManager") as GameObject);
            panelObject.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, true);
            panelObject.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
           InventorySystem invInstance = panelObject.AddComponent<InventorySystem>();
            panelObject.AddComponent<EquipmentSystem>();
            DestroyImmediate(GameObject.FindGameObjectWithTag("DraggingItem"));
            GameObject itemDrag = (GameObject)Instantiate(Resources.Load("Prefabs/DraggingItem") as GameObject);
            panelObject.AddComponent<InventoryDesign>();
            itemDrag.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, true);
           invInstance.getPrefabs();
        }
    }
#endif
}
