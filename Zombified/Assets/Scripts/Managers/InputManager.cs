using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Input", menuName = "Input")]
public class InputManager : ScriptableObject
{
    public bool FPS;
    public KeyCode reloadWeapon = KeyCode.R;
    public KeyCode throwGrenade = KeyCode.G;

    public KeyCode SplitItem;
    public KeyCode InventoryKeyCode;
    public KeyCode StorageKeyCode;
    public KeyCode CharacterSystemKeyCode;
    public KeyCode CraftSystemKeyCode;
}
