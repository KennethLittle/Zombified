using UnityEngine;
using System.Collections;
using static UnityEditor.Progress;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class CreateItemDatabase
{
    public static DataBaseForItems asset;                                                  //The List of all Items

#if UNITY_EDITOR
    public static DataBaseForItems createItemDatabase()                                    //creates a new ItemDatabase(new instance)
    {
        asset = ScriptableObject.CreateInstance<DataBaseForItems>();                       //of the ScriptableObject InventoryItemList

        AssetDatabase.CreateAsset(asset, "Assets/InventoryMaster/Resources/ItemDatabase.asset");            //in the Folder Assets/Resources/ItemDatabase.asset
        AssetDatabase.SaveAssets();                                                         //and than saves it there
        asset.itemNumber.Add(new InventoryItem());
        return asset;
    }
#endif



}
