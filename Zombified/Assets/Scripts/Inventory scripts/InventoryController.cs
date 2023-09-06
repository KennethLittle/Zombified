using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public GameObject inventory;
    public bool InventoryIsClose;

    void Start()
    {
        InventoryIsClose = false;
    }

    void Update()
    {
       if(Input.GetKey(KeyCode.I))
        {
            if(InventoryIsClose == true)
            {
                inventory.SetActive(true);
                InventoryIsClose = false;
            }
            else
            {
                inventory.SetActive(false);
                InventoryIsClose = true;
            }
        }
    }


}
