using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class DragInventory : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    private Vector2 dragOffset;                           // offset of the pointer for dragging
    private RectTransform mainCanvasTransform;            // RectTransform of the parent is needed for dragging
    private RectTransform draggablePanelTransform;        // RectTransform that is getting dragged

    void Awake()
    {
        Canvas mainlayout = GetComponentInParent<Canvas>();              // If the canvas is active, we instantiate the variables
        if (mainlayout != null)
        {
            mainCanvasTransform = mainlayout.transform as RectTransform; // instantiated
            draggablePanelTransform = transform.parent as RectTransform; // instantiated
        }
    }

    public void OnPointerDown(PointerEventData eventData)               // If you press on the Inventory
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            draggablePanelTransform, eventData.position, eventData.pressEventCamera, out dragOffset); // the pointer offset is getting calculated
    }

    public void OnDrag(PointerEventData eventData)                     // If you start dragging now
    {
        if (draggablePanelTransform == null)                            // and no RectTransform from the inventory is there 
            return;                                                     // the function will break out

        Vector2 localDragPosition;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(mainCanvasTransform, Input.mousePosition, eventData.pressEventCamera, out localDragPosition))
        {
            draggablePanelTransform.localPosition = localDragPosition - dragOffset;
        }
    }
}
