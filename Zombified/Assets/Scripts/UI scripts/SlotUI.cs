using System;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Recorder.OutputPath;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;
using UnityEngine.UI;

public class SlotUI : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        DraggableItemUI draggedItem = eventData.pointerDrag.GetComponent<DraggableItemUI>();
        if (draggedItem)
        {
            // Handle moving within the inventory
            if (draggedItem.originalParent.GetComponent<SlotUI>())
            {
                // This item was dragged from another inventory slot
                SlotUI originalSlot = draggedItem.originalParent.GetComponent<SlotUI>();
                originalSlot.ClearSlot(); // You might need to implement this
            }

            draggedItem.transform.SetParent(this.transform);
            draggedItem.transform.localPosition = Vector3.zero;

            bool movedSuccessfully = LootBoxUI.Instance.MoveItemToInventory(draggedItem.AssociatedItem);

            if (movedSuccessfully)
            {
                LootBoxUI.Instance.RemoveItemFromLoot(draggedItem.AssociatedItem);
            }
        }
    }

    public void ClearSlot()
    {
        // Here you'd clear out the visual representation of the item.
        // For instance, if you have an image component to represent the item:

        Image itemImage = GetComponentInChildren<Image>();
        if (itemImage)
        {
            itemImage.sprite = null;
            itemImage.enabled = false;
        }

        // If you are storing the associated item in the SlotUI (like in a private field), clear that too:
        // this.AssociatedItem = null;
    }
}



