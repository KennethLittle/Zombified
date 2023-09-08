using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public GameObject inventory;
    public bool InventoryIsClose;
    public Image image;

    private Transform parentafterDrag;

    private void Start()
    {
        InventoryIsClose = false;
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.I))
        {
            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        inventory.SetActive(!inventory.activeSelf);
        InventoryIsClose = !InventoryIsClose;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        parentafterDrag = transform.parent;
        transform.SetParent(transform.root);
        image.raycastTarget = false;
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
        image.raycastTarget = false;
    }
}
