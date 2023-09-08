using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemManager items;
    [SerializeField] ammoBoxStats ammoBox;


    void Pickup()
    {
        InventoryManager.Instance.AddItem(items);
        Destroy(gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKey(KeyCode.E))
            {
                Pickup();
            }
        }
    }
}
