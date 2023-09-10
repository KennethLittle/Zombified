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
    private float debounceTime = 3f; // Half a second delay
    private float nextToggleTime;

    public void ToggleInventory()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            if (Time.time < nextToggleTime) return;
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


            nextToggleTime = Time.time + debounceTime;
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
            if (slot.childCount == 0)
            {
                GameObject itemUIObject = Instantiate(itemUIPrefab, slot);
                SetIcon(itemUIObject, item.icon);
                return; // Exit the method once the item is added
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
