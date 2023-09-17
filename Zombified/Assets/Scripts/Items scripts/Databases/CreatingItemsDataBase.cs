using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class CreatingItemsDataBase
{
    public static DataBaseForItems asset;

#if Unity_EDITOR

    public static CreatingItemsDataBase CreatingItems()
    {
        asset = ScriptableObject.CreateInstance<DataBaseForItems>();

        AssetDatabase.CreateAsset(asset, "Assets/InventoryMaster/Resources/ItemDatabase.asset");
        AssetDatabase.SaveAssets();
        asset.itemNumber.Add(new InventoryItem());
        return asset;
    }
#endif

}
