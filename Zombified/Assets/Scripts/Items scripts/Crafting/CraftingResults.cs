using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingResults : MonoBehaviour
{
    CraftSystem craftables;
    public int temp = 0;
    GameObject CraftedItem;
    InventorySystem inventory;

    private void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventorySystem>();
        craftables = transform.parent.GetComponent<CraftSystem>();

        CraftedItem = (GameObject)Instantiate(Resources.Load("Prefabs/Item") as GameObject);
        CraftedItem.transform.SetParent(this.gameObject.transform);
        CraftedItem.GetComponent<RectTransform>().localPosition = Vector3.zero;
        CraftedItem.GetComponent<DragItem>().enabled = false;
        CraftedItem.SetActive(false);
        CraftedItem.transform.GetChild(1).GetComponent<Text>().enabled = true;
        CraftedItem.transform.GetChild(1).GetComponent<RectTransform>().localPosition = new Vector2(GameObject.FindGameObjectWithTag("MainInventory").GetComponent<InventorySystem>().positionNumberX, GameObject.FindGameObjectWithTag("MainInventory").GetComponent<InventorySystem>().positionNumberY);
    }

    void Update()
    {
      if(craftables.possibleItems.Count != 0)
      {
          CraftedItem.GetComponent<ItemWithObject>().item = craftables.possibleItems[temp];
          CraftedItem.SetActive(true);
      }
      else
      {
            CraftedItem.SetActive(false);
      }
    }
}
