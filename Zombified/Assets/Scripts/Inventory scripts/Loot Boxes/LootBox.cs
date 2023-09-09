using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using static UnityEditor.Progress;

public class LootBox : MonoBehaviour, iInteractable
{
    public ItemManager itemManager;
    List<ScriptableObject> droppingLoot = new List<ScriptableObject>();
    public TextMeshProUGUI interactTextObject;
    public Transform storage; // A UI container (e.g., a Grid) to hold item UI representations.
    public GameObject itemUIPrefab; // A prefab representing an individual item in the UI.
    private bool isPlayerInRange = false;
    private bool isInteractTextShowing;
    private bool isStorageClosed;
    private const string INTERACT_MESSAGE = "Press E to Open LootBox";

    private void Start()
    {
        isStorageClosed = true;
        droppedLoot();

    }
    private void Update()
    {
        Interact();
    }
    List<ScriptableObject> droppedLoot()
    {
        List<ScriptableObject> loot = new List<ScriptableObject>();

        int amountDropped = UnityEngine.Random.Range(1, 5);

        for (int i = 0; i < amountDropped; i++)
        {
            int dropChance = UnityEngine.Random.Range(1, 101);

            if (dropChance <= 15)
            {
                var med = itemManager.GetItem<ScriptableObject>("MedPack");
                if (med != null) loot.Add(med);
            }
            else if(dropChance <= 25)
            {
                var ammo = itemManager.GetItem<ScriptableObject>("Ammo");
                if (ammo != null) loot.Add(ammo);
            }
            else if (dropChance <= 30)
            {
                var snipe = itemManager.GetItem<ScriptableObject>("Sniper Rifle");
                if (snipe != null) loot.Add(snipe);
            }
            else if (dropChance <= 40)
            {
                var shotgun = itemManager.GetItem<ScriptableObject>("Shotgun");
                if (shotgun != null) loot.Add(shotgun);
            }
            else if (dropChance <= 50)
            {
                var assualtRifle = itemManager.GetItem<ScriptableObject>("Assualt Rifle");
                if (assualtRifle != null) loot.Add(assualtRifle);
            }
            else if (dropChance <= 60)
            {
                var ak = itemManager.GetItem<ScriptableObject>("AK-47");
                if (ak != null) loot.Add(ak);
            }
        }
        return loot;
    }
    public Transform GetTransform()
    {
        return this.transform;
    }

    private void CollectLoot()
    {
        List<ScriptableObject> items = droppedLoot();

        foreach (var item in items)
        {
            GameObject itemUIObject = Instantiate(itemUIPrefab, storage);

            // Set the icon
            Image itemImage = itemUIObject.GetComponentInChildren<Image>();
            if (item is WeaponStats) // Check if the item is of type WeaponStats
            {
                WeaponStats weaponItem = (WeaponStats)item;
                SetIcon(itemUIObject, weaponItem.icon);
            }
            else if (item is medPackStats)
            {
                medPackStats medItem = (medPackStats)item;
                SetIcon(itemUIObject, medItem.icon);
            }
            else if (item is ammoBoxStats)
            {
                ammoBoxStats ammoItem = (ammoBoxStats)item;
                SetIcon(itemUIObject, ammoItem.icon);
            }
            // Here, you'll want to add the item to the player's inventory.
            // Example: PlayerInventory.Add(item);
        }
    }
    private void SetIcon(GameObject uiObject, Sprite icon)
    {
        Image itemImage = uiObject.GetComponentInChildren<Image>();
        if (itemImage != null && icon != null)
        {
            itemImage.sprite = icon;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            ShowInteractText();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            HideInteractText();
        }
    }

    public void Interact()
    {
        //CollectLoot();
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            HideInteractText();
            if (isStorageClosed)
            {
                storage.gameObject.SetActive(true);
                isStorageClosed = false;
            }
            else
            {
                storage.gameObject.SetActive(false);
                isStorageClosed = true;
            }
        }
    }

    public string GetInteractText()
    {
        return INTERACT_MESSAGE;
    }

    private void ShowInteractText()
    {
        isInteractTextShowing = true;
        interactTextObject.gameObject.SetActive(true);
        interactTextObject.text = GetInteractText(); // Sets the text content.
    }

    private void HideInteractText()
    {
        isInteractTextShowing = false;
        interactTextObject.gameObject.SetActive(false);
    }

}

