using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentInput : MonoBehaviour
{
    [SerializeField] GameObject EquipmentObj;
    [SerializeField] GameObject InventoryObj;
    [SerializeField] KeyCode[] EquipmentKey;

    void Update()
    {
        for (int i = 0; i < EquipmentKey.Length; i++)
        {
            if (Input.GetKeyDown(EquipmentKey[i]))
            {
                EquipmentObj.SetActive(!EquipmentObj.activeSelf);
                if (EquipmentObj.activeSelf)
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
    public void ToggleEquipment()
    {
        EquipmentObj.SetActive(!EquipmentObj.activeSelf);
        if (InventoryObj.activeSelf && EquipmentObj.activeSelf)
        {
            GameStateManager.instance.ChangeState(GameStateManager.GameState.Paused);
        }
        else if (!InventoryObj.activeSelf && EquipmentObj.activeSelf)
        {
            GameStateManager.instance.ChangeState(GameStateManager.GameState.Paused);
        }
        else if (!EquipmentObj.activeSelf && InventoryObj.activeSelf)
        {
            GameStateManager.instance.ChangeState(GameStateManager.GameState.Paused);
        }
        else if (!InventoryObj.activeSelf && !EquipmentObj.activeSelf)
        {
            GameStateManager.instance.ChangeState(GameStateManager.GameState.Playing);
        }
    }
}

