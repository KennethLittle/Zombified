using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopUI : MonoBehaviour
{
    public InventoryUI inventoryUI;
    public static ShopUI Instance;
    public Button buyAllButton;

    [System.Serializable]
    public struct ShopItem
    {
        public BaseItemStats item;
        public int cost;
    }

    public Transform storage; // UI container for items
    public GameObject itemUIPrefab;
    public Transform[] slots;
    public List<ShopItem> possibleShopItems;

    private bool isShopClosed = true;
    private List<BaseItemStats> currentShopItems = new List<BaseItemStats>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        PopulateShopItems();
        DisplayShopItems();
        buyAllButton.onClick.AddListener(BuyAllItems);
    }

    void PopulateShopItems()
    {
        // Add logic to determine what items should be in the shop. 
        // For now, we'll just add everything from possibleShopItems.
        foreach (var shopItem in possibleShopItems)
        {
            currentShopItems.Add(shopItem.item);
        }
    }

    public void ToggleUI()
    {
        if (isShopClosed)
        {
            GameStateManager.instance.ChangeState(GameStateManager.GameState.Paused);
            Cursor.visible = true; // This makes the cursor visible
            DisplayShopItems();
            storage.gameObject.SetActive(true);
            isShopClosed = false;
            inventoryUI.OpenInventoryDirectly();
        }
        else
        {
            storage.gameObject.SetActive(false);
            gameManager.instance.inventory.SetActive(false);
            isShopClosed = true;
            GameStateManager.instance.ChangeState(GameStateManager.GameState.Playing);
        }
    }

    private void DisplayShopItems()
    {
        foreach (Transform slot in slots)
        {
            DestroyAllChildren(slot);
        }

        int slotIndex = 0;
        foreach (var item in currentShopItems)
        {
            if (slotIndex < slots.Length)
            {
                GameObject itemUIObject = Instantiate(itemUIPrefab, slots[slotIndex]);
                SetIcon(itemUIObject, item.icon);
                slotIndex++;
            }
            else
            {
                Debug.LogWarning("No more slots available to display shop items.");
                break;
            }
        }
    }

    private void SetIcon(GameObject itemUIObject, Sprite iconSprite)
    {
        Image imageComponent = itemUIObject.GetComponent<Image>();
        if (!imageComponent)
        {
            imageComponent = itemUIObject.GetComponentInChildren<Image>();
        }

        if (imageComponent)
        {
            imageComponent.sprite = iconSprite;
        }
        else
        {
            Debug.LogError("No Image component found for: " + itemUIObject.name);
        }
    }

    public void BuyItemFromShop(BaseItemStats item, int cost)
    {
        // Implement logic to buy item, which may involve checking player's money, etc.
        // For now, we just move the item to inventory:
        inventoryUI.AddItemToInventory(item);

        // Remove the item from currentShopItems
        currentShopItems.Remove(item);
        DisplayShopItems();
    }

    public void BuyAllItems()
    {
        foreach (var item in currentShopItems)
        {
            BuyItemFromShop(item, possibleShopItems.Find(si => si.item == item).cost);
        }
        currentShopItems.Clear();
        DisplayShopItems();
    }

    private void DestroyAllChildren(Transform parentTransform)
    {
        for (int i = parentTransform.childCount - 1; i >= 0; i--)
        {
            Transform child = parentTransform.GetChild(i);
            Destroy(child.gameObject);
        }
    }
}