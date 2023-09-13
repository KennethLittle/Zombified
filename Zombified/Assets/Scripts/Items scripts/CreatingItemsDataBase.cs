using UnityEngine;
using System.Collections;
using UnityEditor;

public class CreatingItemsDataBase
{
    public static CreatingItemsDataBase asset;

    public static CreatingItemsDataBase CreatingItems()
    {
        //asset = ScriptableObject.CreateInstance<CreatingItemsDataBase>();

        //AssetDatabase.CreatingAsset(asset, "Assets/InventoryMaster/Resources/ItemDatabase.asset");
        //AssetDatabase.SaveAssets();
        //asset.itemList.Add(new Item());
        return asset;
    }
}
