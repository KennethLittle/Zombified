using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagSlot : MonoBehaviour
{
    public Image Icon;
    public Button removeButton;

    private InventoryObject item;

    public void AddObject(InventoryObject weapon)
    {
        item = weapon;
        Icon.sprite = weapon.WeaponIcon;
        Icon.enabled = true;
        removeButton.interactable = true;
    }

    public void ClearSlot()
    {
        item = null;
        Icon.sprite = null;
        Icon.enabled = false;
        removeButton.interactable = false;
    }

    public void OnRemoveButton()
    {
        if(item != null)
        {
            InventorySetup.Instance.RemoveItem(item);
            ClearSlot();
        }
    }
}
