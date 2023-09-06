using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [HideInInspector]
    public InventorySlot selectedInventorySlot;

    private void Update()
    {
        if(selectedInventorySlot == null)
        {
            return;
        }
        if(Input.GetMouseButton(0))
        {
           
        }
    }
}
