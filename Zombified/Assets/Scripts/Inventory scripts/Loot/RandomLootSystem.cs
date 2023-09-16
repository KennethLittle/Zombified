using System.Collections;
using UnityEngine;

public class RandomLootSystem : MonoBehaviour
{
    public int amountOfLoot = 10;
    static DataBaseForItems inventoryItemList;

    int counter = 0;
    // Use this for initialization
    void Start()
    {
        inventoryItemList = (DataBaseForItems)Resources.Load("ItemDatabase");
        while (counter < amountOfLoot)
        {
            counter++;
            int randomNumber = Random.Range(1, inventoryItemList.itemNumber.Count - 1);
            Terrain terrain = Terrain.activeTerrain;

            float x = Random.Range(5, terrain.terrainData.size.x - 5);
            float z = Random.Range(5, terrain.terrainData.size.z - 5);
            if (inventoryItemList.itemNumber[randomNumber].itemModel == null)
            {
                counter--;
            }
            else
            {
                GameObject randomLootItem = (GameObject)Instantiate(inventoryItemList.itemNumber[randomNumber].itemModel);
                ItemPickup item = randomLootItem.AddComponent<ItemPickup>();
                item.item = inventoryItemList.itemNumber[randomNumber];
                randomLootItem.transform.localPosition=new Vector3(x,0,z);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
