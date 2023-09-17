using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Input", menuName = "Input")]
public class InputManager : ScriptableObject
{
    public bool FPS;
    public KeyCode reloadWeapon = KeyCode.R;
    public KeyCode throwGrenade = KeyCode.G;

    public KeyCode SplitItem = KeyCode.B;
    public KeyCode InventoryKeyCode = KeyCode.I;
    public KeyCode StorageKeyCode = KeyCode.N;
    public KeyCode CharacterSystemKeyCode = KeyCode.C;
    public KeyCode CraftSystemKeyCode = KeyCode.X;
}
