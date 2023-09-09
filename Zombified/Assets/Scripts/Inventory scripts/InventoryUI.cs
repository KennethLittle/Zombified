using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public bool InventoryIsClose;
    public  List<Image> InventorySlots = new List<Image>();

    private Transform parentafterDrag;


    public void ToggleInventory()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            if (InventoryIsClose == true)
            {
                gameManager.instance.inventory.SetActive(true);
                InventoryIsClose = false;
            }
            else
            {
                gameManager.instance.inventory.SetActive(false);
                InventoryIsClose = true;
            }
        }
    }

        public void OnBeginDrag(PointerEventData eventData)
    {
        parentafterDrag = transform.parent;
        transform.SetParent(transform.root);
        foreach(Image img in InventorySlots)
        {
            img.raycastTarget = true;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(transform.childCount == 0)
        {
            GameObject dropped = eventData.pointerDrag;
            DragItem movedItem = dropped.GetComponent<DragItem>();
            if(movedItem)
            {
                movedItem.parentafterDrag = transform;
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentafterDrag);
        foreach (Image img in InventorySlots)
        {
            img.raycastTarget = false;
        }

    }
}
