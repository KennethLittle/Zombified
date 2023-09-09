using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public bool InventoryIsClose;
    public List<Transform> InventorySlots = new List<Transform>();
    public GameObject itemUIPrefab;

    public void ToggleInventory()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            if (InventoryIsClose == true)
            {
                Cursor.lockState = CursorLockMode.None; // This unlocks the cursor
                Cursor.visible = true; // This makes the cursor visible
                gameManager.instance.inventory.SetActive(true);
                InventoryIsClose = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked; // This locks the cursor to the center
                Cursor.visible = false; // This hides the cursor
                gameManager.instance.inventory.SetActive(false);
                InventoryIsClose = true;
            }
        }
    }

    public void OpenInventoryDirectly()
    {
        gameManager.instance.inventory.SetActive(true);
        InventoryIsClose = false;
    }

    public void AddItemToInventory(BaseItemStats item)
    {
        foreach (var slot in InventorySlots)
        {
            // Check if the slot is empty using the Transform component's childCount property
            if (slot.childCount == 0)
            {
                GameObject itemUIObject = Instantiate(itemUIPrefab, slot); // The slot is already a Transform, so this is fine.
                SetIcon(itemUIObject, item.icon);
                return;
            }
        }
        Debug.LogWarning("No available slots in the inventory!");
    }

    private void SetIcon(GameObject itemUIObject, Sprite iconSprite)
    {
        Image imageComponent = itemUIObject.GetComponent<Image>();
        if (!imageComponent)
        {
            imageComponent = itemUIObject.GetComponentInChildren<Image>();
        }

        if (imageComponent)
        {
            imageComponent.sprite = iconSprite;
        }
        else
        {
            Debug.LogError("No Image component found for: " + itemUIObject.name);
        }
    }
}
