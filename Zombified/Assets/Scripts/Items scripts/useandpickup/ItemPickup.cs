using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{

    public InventoryItem item;
    private InventorySystem inventorysetup;
    private GameObject player;
    private bool playerInRange = false;
    // Use this for initialization

    void Start()
    {
        GameObject playerManager = GameObject.FindGameObjectWithTag("PlayerManager");

        if (playerManager != null)
        {
            PlayerEquipment playerEquipment = playerManager.GetComponent<PlayerEquipment>();
            if (playerEquipment != null)
            {
                inventorysetup = playerEquipment.inventorysetup.GetComponent<InventorySystem>();
            }

            Transform playerTransform = playerManager.transform.Find("Player");
            if (playerTransform != null)
            {
                player = playerTransform.gameObject;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    void Update()
    {
        if (playerInRange && inventorysetup != null && Input.GetKeyDown(KeyCode.E))
        {
            bool check = inventorysetup.checkIfItemAllreadyExist(item.itemID, item.itemValue);
            if (check)
                Destroy(this.gameObject);
            else if (inventorysetup.ItemsInInventory.Count < (inventorysetup.width * inventorysetup.height))
            {
                inventorysetup.addItemToInventory(item.itemID, item.itemValue);
                QuestManager.instance.NotifyItemFound(this.gameObject);
                inventorysetup.updateItemList();
                inventorysetup.UpdateStackableItems();
                Destroy(this.gameObject);
            }
        }
    }

}