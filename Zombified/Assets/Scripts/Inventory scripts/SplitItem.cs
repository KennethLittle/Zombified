using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SplitItem : MonoBehaviour, IPointerDownHandler
{     //splitting an Item

    private bool pressingButtonToSplit;             //bool for pressing a item to split it
    public InventorySystem inventory;                          //inventory script  
    static InputManager inputManagerDatabase = null;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))                     //if we press right controll the ....
            pressingButtonToSplit = true;                               //getting changed to true 
        if (Input.GetKeyUp(KeyCode.N))
            pressingButtonToSplit = false;                              //or false

    }

    void Start()
    {
        inputManagerDatabase = (InputManager)Resources.Load("InputManager");
    }

    public void OnPointerDown(PointerEventData data)                    //splitting the item now
    {
        inventory = transform.parent.parent.parent.GetComponent<InventorySystem>();
        if (transform.parent.parent.parent.GetComponent<ToolBar>() == null && data.button == PointerEventData.InputButton.Left && pressingButtonToSplit && inventory.stackable && (inventory.ItemsInInventory.Count < (inventory.height * inventory.width))) //if you press leftclick and and keycode
        {
            ItemWithObject itemOnObject = GetComponent<ItemWithObject>();                                                   //we take the ItemOnObject script of the item in the slot

            if (itemOnObject.item.itemValue > 1)                                                                         //we split the item only when we have more than 1 in the stack
            {
                int splitPart = itemOnObject.item.itemValue;                                                           //we take the value and store it in there
                itemOnObject.item.itemValue = (int)itemOnObject.item.itemValue / 2;                                     //calculate the new value for the splitted item
                splitPart = splitPart - itemOnObject.item.itemValue;                                                   //take the different

                inventory.addItemToInventory(itemOnObject.item.itemID, splitPart);                                            //and add a new item to the inventory
                inventory.UpdateStackableItems();

                if (GetComponent<ConsumingItems>().duplicateItem != null)
                {
                    GameObject dup = GetComponent<ConsumingItems>().duplicateItem;
                    dup.GetComponent<ItemWithObject>().item.itemValue = itemOnObject.item.itemValue;
                    dup.GetComponent<SplitItem>().inventory.UpdateStackableItems();
                }
                inventory.updateItemList();

            }
        }
    }
}
