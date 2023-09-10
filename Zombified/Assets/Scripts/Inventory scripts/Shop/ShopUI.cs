using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopUI : MonoBehaviour
{
    private Transform Shop;
    private Transform shopItemTemplate;
    private Buying purchase;

    private void Awake()
    {
        Shop = transform.Find("Shop");
        shopItemTemplate = Shop.Find("ShopItemTemplate");
        shopItemTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        //CreateItemButton(BaseItemStats.ItemType.AK,BaseItemStats.GetSprite(BaseItemStats.ItemType.AK), "AK - 47", BaseItemStats.GetCost(BaseItemStats.ItemType.AK), 0);
        //CreateItemButton(BaseItemStats.ItemType.AssaultRifle,BaseItemStats.GetSprite(BaseItemStats.ItemType.AssaultRifle), "Assault Rifle", BaseItemStats.GetCost(BaseItemStats.ItemType.AssaultRifle), 1);
        //CreateItemButton(BaseItemStats.ItemType.Shotgun,BaseItemStats.GetSprite(BaseItemStats.ItemType.Shotgun), "Shotgun", BaseItemStats.GetCost(BaseItemStats.ItemType.Shotgun), 1);
        //CreateItemButton(BaseItemStats.ItemType.Sniper,BaseItemStats.GetSprite(BaseItemStats.ItemType.Sniper), "Sniper", BaseItemStats.GetCost(BaseItemStats.ItemType.Sniper), 1);
        //CreateItemButton(BaseItemStats.ItemType.AmmoBox,BaseItemStats.GetSprite(BaseItemStats.ItemType.AmmoBox), "Ammo Box", BaseItemStats.GetCost(BaseItemStats.ItemType.AmmoBox), 1);
        //CreateItemButton(BaseItemStats.ItemType.MedPack,BaseItemStats.GetSprite(BaseItemStats.ItemType.MedPack), "MedPack", BaseItemStats.GetCost(BaseItemStats.ItemType.MedPack), 1);

    }
//    private void CreateItemButton(BaseItemStats.ItemType itemtype, Sprite itemSprite, string itemName, int Buyable, int positionlocation)
//    {
//        Transform shopItemTransform = Instantiate(shopItemTemplate, Shop);
//        shopItemTransform.gameObject.SetActive(true);
//        RectTransform shopItemRectTransform = shopItemTransform.GetComponent<RectTransform>();

//        float shopItemHeight = 90f;
//        shopItemRectTransform.anchoredPosition = new Vector2(0, -shopItemHeight * positionlocation);

//        shopItemTransform.Find("Weapon").GetComponent<TextMeshProUGUI>().SetText(itemName);
//        shopItemTransform.Find("Price").GetComponent<TextMeshProUGUI>().SetText(Buyable.ToString());
//        shopItemTransform.Find("Weapon Image").GetComponent<Image>().sprite = itemSprite;

//        //shopItemTransform.GetComponent<Button>().onClick = () =>
//        //{ BuyItem(type)}
//    }

//    private void BuyItem(BaseItemStats.ItemType type)
//    {
//        purchase.BuyingItem(type);
//    }

//    public void Show(Buying buy)
//    {
//        this.purchase = buy;
//        gameObject.SetActive(true);
//    }

//    public void Hide()
//    {
//        gameObject.SetActive(false);
//    }
}
