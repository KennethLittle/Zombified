using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CreateInputManager : MonoBehaviour
{
    public static InputManager asset;

#if UNITY_EDITOR
    public static InputManager createInputManager()
    {
        asset = ScriptableObject.CreateInstance<InputManager>();

        AssetDatabase.CreateAsset(asset, "Assets/InventoryMaster/Resources/InputManager.asset");
        AssetDatabase.SaveAssets();
        return asset;
    }
#endif

}

