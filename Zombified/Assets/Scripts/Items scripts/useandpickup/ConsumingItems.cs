using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using static UnityEditor.Progress;

public class ConsumingItems : MonoBehaviour, IPointerDownHandler
{
    public InventoryItem currentitem;
    private static HelperTool itemhelper;
    public ItemType[] slotItemTypes;
    public static EquipmentSystem equipmentSystem;
    public GameObject duplicateItem;
    public static GameObject primaryInventory;

    void Start()
    {
        currentitem = GetComponent<ItemWithObject>().item;
        if (GameObject.FindGameObjectWithTag("CharacterSetup") != null)
            equipmentSystem = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerEquipment>().charactersetup.GetComponent<EquipmentSystem>();
        if (GameObject.FindGameObjectWithTag("InventorySetup") != null)
            primaryInventory = GameObject.FindGameObjectWithTag("InventorySetup");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (this.gameObject.transform.parent.parent.parent.GetComponent<EquipmentSystem>() == null)
        {
            bool gearable = false;
            InventorySystem inventory = transform.parent.parent.parent.GetComponent<InventorySystem>();

            if (equipmentSystem != null)
                slotItemTypes = equipmentSystem.slotItemTypes;

            if (eventData.button == PointerEventData.InputButton.Right)
            {
                //item from craft system to inventory
                if (transform.parent.GetComponent<CraftingResults>() != null)
                {
                    bool check = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerEquipment>().inventorysetup.GetComponent<InventorySystem>().checkIfItemAllreadyExist(currentitem.itemID, currentitem.itemValue);

                    if (!check)
                    {
                        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerEquipment>().inventorysetup.GetComponent<InventorySystem>().addItemToInventory(currentitem.itemID, currentitem.itemValue);
                        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerEquipment>().inventorysetup.GetComponent<InventorySystem>().UpdateStackableItems();
                    }
                    CraftSystem cS = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerEquipment>().craftSystem.GetComponent<CraftSystem>();
                    cS.deleteItems(currentitem);
                    CraftingResults result = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerEquipment>().craftSystem.transform.GetChild(3).GetComponent<CraftingResults>();
                    result.temp = 0;
                    itemhelper.deactivateHelperTool();
                    gearable = true;
                    GameObject.FindGameObjectWithTag("MainInventory").GetComponent<InventorySystem>().updateItemList();
                }
                else
                {
                    bool stop = false;
                    if (equipmentSystem != null)
                    {
                        for (int i = 0; i < equipmentSystem.totalSlots; i++)
                        {
                            if (slotItemTypes[i].Equals(currentitem.itemType))
                            {
                                if (equipmentSystem.transform.GetChild(1).GetChild(i).childCount == 0)
                                {
                                    stop = true;
                                    if (equipmentSystem.transform.GetChild(1).GetChild(i).parent.parent.GetComponent<EquipmentSystem>() != null && this.gameObject.transform.parent.parent.parent.GetComponent<EquipmentSystem>() != null) { }
                                    else
                                        inventory.EquiptItem(currentitem);

                                    this.gameObject.transform.SetParent(equipmentSystem.transform.GetChild(1).GetChild(i));
                                    this.gameObject.GetComponent<RectTransform>().localPosition = Vector3.zero;
                                    equipmentSystem.gameObject.GetComponent<InventorySystem>().updateItemList();
                                    inventory.updateItemList();
                                    gearable = true;
                                    if (duplicateItem != null)
                                        Destroy(duplicateItem.gameObject);
                                    break;
                                }
                            }
                        }


                        if (!stop)
                        {
                            for (int i = 0; i < equipmentSystem.totalSlots; i++)
                            {
                                if (slotItemTypes[i].Equals(currentitem.itemType))
                                {
                                    if (equipmentSystem.transform.GetChild(1).GetChild(i).childCount != 0)
                                    {
                                        GameObject otherItemFromCharacterSystem = equipmentSystem.transform.GetChild(1).GetChild(i).GetChild(0).gameObject;
                                        InventoryItem otherSlotItem = otherItemFromCharacterSystem.GetComponent<ItemWithObject>().item;
                                        if (currentitem.itemType == ItemType.Weapon)
                                        {
                                            inventory.UnEquipItem1(otherItemFromCharacterSystem.GetComponent<ItemWithObject>().item);
                                            inventory.EquiptItem(currentitem);
                                        }
                                        else
                                        {
                                            inventory.EquiptItem(currentitem);
                                            if (currentitem.itemType != ItemType.Backpack)
                                                inventory.UnEquipItem1(otherItemFromCharacterSystem.GetComponent<ItemWithObject>().item);
                                        }
                                        if (this == null)
                                        {
                                            GameObject dropItem = (GameObject)Instantiate(otherSlotItem.itemModel);
                                            dropItem.AddComponent<ItemPickup>();
                                            dropItem.GetComponent<ItemPickup>().item = otherSlotItem;
                                            dropItem.transform.localPosition = GameObject.FindGameObjectWithTag("Player").transform.localPosition;
                                            inventory.OnUpdateItemList();
                                        }
                                        else
                                        {
                                            otherItemFromCharacterSystem.transform.SetParent(this.transform.parent);
                                            otherItemFromCharacterSystem.GetComponent<RectTransform>().localPosition = Vector3.zero;
                                            if (this.gameObject.transform.parent.parent.parent.GetComponent<ToolBar>() != null)
                                                CreateDuplicate(otherItemFromCharacterSystem);

                                            this.gameObject.transform.SetParent(equipmentSystem.transform.GetChild(1).GetChild(i));
                                            this.gameObject.GetComponent<RectTransform>().localPosition = Vector3.zero;
                                        }

                                        gearable = true;
                                        if (duplicateItem != null)
                                            Destroy(duplicateItem.gameObject);
                                        equipmentSystem.gameObject.GetComponent<InventorySystem>().updateItemList();
                                        inventory.OnUpdateItemList();
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public void ConsumeCurrentItem()
    {
        InventorySystem inventory = transform.parent.parent.parent.GetComponent<InventorySystem>();

        bool gearable = false;

        if (GameObject.FindGameObjectWithTag("EquipmentSystem") != null)
            equipmentSystem = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerEquipment>().charactersetup.GetComponent<EquipmentSystem>();

        if (equipmentSystem != null)
            slotItemTypes = equipmentSystem.slotItemTypes;

        InventoryItem itemFromDup = null;
        if (duplicateItem != null)
            itemFromDup = duplicateItem.GetComponent<ItemWithObject>().item;

        bool stop = false;
        if (equipmentSystem != null)
        {

            for (int i = 0; i < equipmentSystem.totalSlots; i++)
            {
                if (slotItemTypes[i].Equals(currentitem.itemType))
                {
                    if (equipmentSystem.transform.GetChild(1).GetChild(i).childCount == 0)
                    {
                        stop = true;
                        this.gameObject.transform.SetParent(equipmentSystem.transform.GetChild(1).GetChild(i));
                        this.gameObject.GetComponent<RectTransform>().localPosition = Vector3.zero;
                        equipmentSystem.gameObject.GetComponent<InventorySystem>().updateItemList();
                        inventory.updateItemList();
                        inventory.EquiptItem(currentitem);
                        gearable = true;
                        if (duplicateItem != null)
                            Destroy(duplicateItem.gameObject);
                        break;
                    }
                }
            }

            if (!stop)
            {
                for (int i = 0; i < equipmentSystem.totalSlots; i++)
                {
                    if (slotItemTypes[i].Equals(currentitem.itemType))
                    {
                        if (equipmentSystem.transform.GetChild(1).GetChild(i).childCount != 0)
                        {
                            GameObject otherItemFromCharacterSystem = equipmentSystem.transform.GetChild(1).GetChild(i).GetChild(0).gameObject;
                            InventoryItem otherSlotItem = otherItemFromCharacterSystem.GetComponent<ItemWithObject>().item;
                            if (currentitem.itemType == ItemType.Weapon)
                            {
                                inventory.UnEquipItem1(otherItemFromCharacterSystem.GetComponent<ItemWithObject>().item);
                                inventory.EquiptItem(currentitem);
                            }
                            else
                            {
                                inventory.EquiptItem(currentitem);
                                if (currentitem.itemType != ItemType.Backpack)
                                    inventory.UnEquipItem1(otherItemFromCharacterSystem.GetComponent<ItemWithObject>().item);
                            }
                            if (this == null)
                            {
                                GameObject dropItem = (GameObject)Instantiate(otherSlotItem.itemModel);
                                dropItem.AddComponent<ItemPickup>();
                                dropItem.GetComponent<ItemPickup>().item = otherSlotItem;
                                dropItem.transform.localPosition = GameObject.FindGameObjectWithTag("Player").transform.localPosition;
                                inventory.OnUpdateItemList();
                            }
                            else
                            {
                                otherItemFromCharacterSystem.transform.SetParent(this.transform.parent);
                                otherItemFromCharacterSystem.GetComponent<RectTransform>().localPosition = Vector3.zero;
                                if (this.gameObject.transform.parent.parent.parent.GetComponent<ToolBar>() != null)
                                    CreateDuplicate(otherItemFromCharacterSystem);

                                this.gameObject.transform.SetParent(equipmentSystem.transform.GetChild(1).GetChild(i));
                                this.gameObject.GetComponent<RectTransform>().localPosition = Vector3.zero;
                            }

                            gearable = true;
                            if (duplicateItem != null)
                                Destroy(duplicateItem.gameObject);
                            equipmentSystem.gameObject.GetComponent<InventorySystem>().updateItemList();
                            inventory.OnUpdateItemList();
                            break;
                        }
                    }
                }
            }
        }
        if (!gearable && currentitem.itemType != ItemType.Weapon)
        {

            if (duplicateItem != null)
                itemFromDup = duplicateItem.GetComponent<ItemWithObject>().item;

            inventory.ConsumeItem(currentitem);


            currentitem.itemValue--;
            if (itemFromDup != null)
            {
                duplicateItem.GetComponent<ItemWithObject>().item.itemValue--;
                if (itemFromDup.itemValue <= 0)
                {
                    if (itemhelper != null)
                        itemhelper.deactivateHelperTool();
                    inventory.deleteItemFromInventory(currentitem);
                    Destroy(duplicateItem.gameObject);

                }
            }
            if (currentitem.itemValue <= 0)
            {
                if (itemhelper != null)
                   itemhelper.deactivateHelperTool();
                inventory.deleteItemFromInventory(currentitem);
                Destroy(this.gameObject);
            }

        }
    }

    public void CreateDuplicate(GameObject targetItem)
    {
        InventoryItem itemReference = targetItem.GetComponent<ItemWithObject>().item;
        GameObject duplicate = primaryInventory.GetComponent<InventorySystem>().addItemToInventory(itemReference.itemID, itemReference.itemValue);
        targetItem.GetComponent<ConsumingItems>().duplicateItem = duplicate;
        duplicate.GetComponent<ConsumingItems>().duplicateItem = targetItem;
    }
}
