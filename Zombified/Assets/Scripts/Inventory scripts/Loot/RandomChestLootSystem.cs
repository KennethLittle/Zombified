using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class RandomChestLootSystem : MonoBehaviour
{


    public int amountOfChest = 10;

    public int minItemInChest = 2;
    public int maxItemInChest = 10;

    static DataBaseForItems inventoryItemList;

    public GameObject storageBox;

    int counter;
    int creatingItemsForChest = 0;
    int randomItemNumber;


    // Use this for initialization
    void Start()
    {

        inventoryItemList = (DataBaseForItems)Resources.Load("ItemDatabase");

        while (counter < amountOfChest)
        {
            counter++;

            creatingItemsForChest = 0;

            int itemAmountForChest = Random.Range(minItemInChest, maxItemInChest);
            List<InventoryItem> itemsForChest = new List<InventoryItem>();

            while (creatingItemsForChest < itemAmountForChest)
            {
                randomItemNumber = Random.Range(1, inventoryItemList.itemNumber.Count - 1);
                int raffle = Random.Range(1, 100);

                if (raffle <= inventoryItemList.itemNumber[randomItemNumber].rarity)
                {
                    itemsForChest.Add(inventoryItemList.itemNumber[randomItemNumber].getCopy());
                    creatingItemsForChest++;
                }
            }


            Terrain terrain = Terrain.activeTerrain;

            float x = Random.Range(5, terrain.terrainData.size.x - 5);
            float z = Random.Range(5, terrain.terrainData.size.z - 5);

            float height = terrain.terrainData.GetHeight((int)x, (int)z);

            GameObject chest = (GameObject)Instantiate(storageBox);
            InventoryStorage sI = chest.GetComponent<InventoryStorage>();
            sI.inventory = GameObject.FindGameObjectWithTag("Storage");

            for (int i = 0; i < itemsForChest.Count; i++)
            {
                sI.storedItems.Add(inventoryItemList.getItemByID(itemsForChest[i].itemID));

                int randomValue = Random.Range(1, sI.storedItems[sI.storedItems.Count - 1].maxStack);
                sI.storedItems[sI.storedItems.Count - 1].itemValue = randomValue;
            }

            chest.transform.localPosition = new Vector3(x, height + 2, z);
        }

    }

}
