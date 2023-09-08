using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LootBox : MonoBehaviour, iInteractable
{
    public ItemManager itemManager;
    public TextMeshProUGUI interactTextObject;
    private bool isPlayerInRange = false;
    private const string INTERACT_MESSAGE = "Press E to Open LootBox";

    private void Start()
    {

        droppedLoot();

    }
    private void Update()
    {
        if (isPlayerInRange)
        {
            ShowInteractText();
        }
        else
        {
            HideInteractText();
        }
    }
    List<ScriptableObject> droppedLoot()
    {
        int amountDropped = Random.Range(1, 5);

        List<ScriptableObject> droppingLoot = new List<ScriptableObject>();

        for (int i = 0; i < amountDropped; i++)
        {
            int dropChance = Random.Range(1, 101);

            if (dropChance <= 15)
            {
                medPackStats med = itemManager.GetItem<medPackStats>("MedPack");
                droppingLoot.Add(med);
            }
            else if(dropChance <= 25)
            {
                ammoBoxStats ammo = itemManager.GetItem<ammoBoxStats>("Ammo");
                droppingLoot.Add(ammo);
            }
            else if (dropChance <= 30)
            {
                WeaponStats snipe = itemManager.GetItem<WeaponStats>("Sniper Rifle");
                droppingLoot.Add(snipe);
            }
            else if (dropChance <= 40)
            {
                WeaponStats shotgun = itemManager.GetItem<WeaponStats>("Shotgun");
                droppingLoot.Add(shotgun);
            }
            else if (dropChance <= 50)
            {
                WeaponStats assualtRifle = itemManager.GetItem<WeaponStats>("Assualt Rifle");
                droppingLoot.Add(assualtRifle);
            }
            else if (dropChance <= 60)
            {
                WeaponStats ak = itemManager.GetItem<WeaponStats>("AK-47");
                droppingLoot.Add(ak);
            }
        }
        return droppingLoot;
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
            // Here, you'll want to add the item to the player's inv.
            // Example: PlayerInventory.Add(item);

            // Optionally, provide feedback to the player about what they collected.
        }

        // Optionally destroy the loot box after the loot has been collected
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }

    public void Interact()
    {
        if (isPlayerInRange)
        {
            CollectLoot();
            HideInteractText();
        }
    }

    public string GetInteractText()
    {
        return INTERACT_MESSAGE;
    }

    private void ShowInteractText()
    {
        interactTextObject.gameObject.SetActive(true);
        interactTextObject.text = GetInteractText(); // Sets the text content.
    }

    private void HideInteractText()
    {
        interactTextObject.gameObject.SetActive(false);
    }

}

