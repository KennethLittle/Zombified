using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(InventorySlot))]
public class InventoryInteract : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    InventoryController inventoryController;
    InventorySlot inventorySlot;

    private void Awake()
    {
        inventoryController = FindObjectOfType(typeof(InventoryController)) as InventoryController;
        inventorySlot = GetComponent<InventorySlot>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        inventoryController.selectedInventorySlot = inventorySlot;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        inventoryController.selectedInventorySlot = null;
    }

}
