using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public Button removeButton;

    private Inventoryitem item;

    public void AddItem(Inventoryitem newitem)
    {
        item = newitem;
        icon.sprite = newitem.itemIcon;
        icon.enabled = true;
        removeButton.interactable = true;
    }

    public void ClearSlot()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        removeButton.interactable = false;
    }

    public void OnRemoveButton()
    {
        if(item != null)
        {
            InventoryManager.Instance.RemoveItem(item);
            ClearSlot();
        }
    }
}
