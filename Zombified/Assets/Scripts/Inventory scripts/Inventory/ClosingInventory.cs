using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClosingInventory : MonoBehaviour, IPointerDownHandler
{
    InventorySystem inventorySystem;

    // Start is called before the first frame update
    void Start()
    {
        inventorySystem = transform.parent.GetComponent<InventorySystem>();
    }
    public void OnPointerDown(PointerEventData pointerinfo)
    {
        if(pointerinfo.button == PointerEventData.InputButton.Left)
        {
            inventorySystem.closeInventory();
        }
    }

}
