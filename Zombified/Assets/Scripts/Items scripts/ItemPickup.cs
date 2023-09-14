using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{

    public InventoryItem item;
    private InventorySystem inventorysetup;
    private GameObject player;
    // Use this for initialization

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            inventorysetup = player.GetComponent<PlayerEquipment>().inventorysetup.GetComponent<InventorySystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inventorysetup != null && Input.GetKeyDown(KeyCode.E))
        {
            float distance = Vector3.Distance(this.gameObject.transform.position, player.transform.position);

            if (distance <= 3)
            {
                bool check = inventorysetup.checkIfItemAllreadyExist(item.itemID, item.itemValue);
                if (check)
                    Destroy(this.gameObject);
                else if (inventorysetup.ItemsInInventory.Count < (inventorysetup.width * inventorysetup.height))
                {
                    inventorysetup.addItemToInventory(item.itemID, item.itemValue);
                    inventorysetup.updateItemList();
                    inventorysetup.stackableSettings();
                    Destroy(this.gameObject);
                }

            }
        }
    }

}