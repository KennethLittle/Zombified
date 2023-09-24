using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryInput : MonoBehaviour
{
    [SerializeField] GameObject InventoryObj;
    [SerializeField] GameObject EquipmentObj;
    [SerializeField] KeyCode[] InventoryKey;

    private void Update()
    {
        for(int i = 0; i < InventoryKey.Length;i++)
        {
            if (Input.GetKeyDown(InventoryKey[i]))
            {
                InventoryObj.SetActive(!InventoryObj.activeSelf);
                if (InventoryObj.activeSelf)
                {
                    ShowMouse();
                }
                else
                {
                    HideMouse();
                }

                break;
            }
        }
    }
    public void ShowMouse()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        GameStateManager.instance.ChangeState(GameStateManager.GameState.Paused);

    }

    public void HideMouse()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        GameStateManager.instance.ChangeState(GameStateManager.GameState.Playing);
    }
    public void ToggleInventory()
    {
        InventoryObj.SetActive(!InventoryObj.activeSelf);
        if (InventoryObj.activeSelf && EquipmentObj.activeSelf)
        {
            GameStateManager.instance.ChangeState(GameStateManager.GameState.Paused);
        }
        else if(!InventoryObj.activeSelf && EquipmentObj.activeSelf) 
        {
            GameStateManager.instance.ChangeState(GameStateManager.GameState.Paused);
        }
        else if(!EquipmentObj.activeSelf && InventoryObj.activeSelf)
        {
            GameStateManager.instance.ChangeState(GameStateManager.GameState.Paused);
        }
        else if(!InventoryObj.activeSelf && !EquipmentObj.activeSelf)
        {
            GameStateManager.instance.ChangeState(GameStateManager.GameState.Playing);
        }
    }

}
