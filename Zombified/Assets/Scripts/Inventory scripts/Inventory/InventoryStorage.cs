using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;

public class InventoryStorage : MonoBehaviour
{
    [SerializeField]
    public GameObject inventory;

    [SerializeField]
    public List<InventoryItem> storedItems = new List<InventoryItem>();

    [SerializeField]
    private DataBaseForItems itemDatabase;

    [SerializeField]
    private int interactionDistance;

    public float interactionDelay;

    private InputManager inputManager;

    private float interactionStart;
    private float interactionEnd;
    private bool displayInteractionTimer;

    public int maximumRandomItems;

    private HelperTool itemHelper;
    private InventorySystem invent;

    private GameObject player;

    private static Image interactionTimerImage;
    private static GameObject interactionTimerObject;

    private bool closeInventoryFlag;

    private bool isInteractingWithStorage;

    private bool playerInRange = false;


    public void AddItemToStorage(int id, int quantity)
    {
        InventoryItem itemToAdd = itemDatabase.getItemByID(id);
        itemToAdd.itemValue = quantity;
        storedItems.Add(itemToAdd);
    }

    void Start()
    {
        if (inputManager == null)
            inputManager = (InputManager)Resources.Load("InputManager");

        player = GameObject.FindGameObjectWithTag("Player");
        invent = inventory.GetComponent<InventorySystem>();
        DataBaseForItems itemDB = (DataBaseForItems)Resources.Load("ItemDatabase");

        int itemsToGenerate = Random.Range(1, maximumRandomItems);
        int currentItemCount = 1;

        while (currentItemCount < itemsToGenerate)
        {
            int randomItemID = Random.Range(1, itemDB.itemNumber.Count - 1);
            int chance = Random.Range(1, 100);

            if (chance <= itemDB.itemNumber[randomItemID].rarity)
            {
                int randomQuantity = Random.Range(1, itemDB.itemNumber[randomItemID].getCopy().maxStack);
                InventoryItem randomItem = itemDB.itemNumber[randomItemID].getCopy();
                randomItem.itemValue = randomQuantity;
                storedItems.Add(randomItem);
                currentItemCount++;
            }
        }
    }

    public void InitializeImportantVariables()
    {
        if (itemDatabase == null)
            itemDatabase = (DataBaseForItems)Resources.Load("ItemDatabase");
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  // Make sure your player has the tag "Player"
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            // This section ensures the storage closes if the player walks away
            if (isInteractingWithStorage)
            {
                storedItems.Clear();
                UpdateStoredItemsList();
                invent.RemoveAllItems();
            }
        }
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            ToggleStorageInteraction();
        }
    }

    void ToggleStorageInteraction()
    {
        isInteractingWithStorage = !isInteractingWithStorage;
        QuestManager.instance.NotifyObjectInteracted(this.gameObject);
        if (isInteractingWithStorage)
        {
            invent.ItemsInInventory.Clear();
            inventory.SetActive(true);
            TransferItemsToInventory();
        }
        else
        {
            storedItems.Clear();
            UpdateStoredItemsList();
            inventory.SetActive(false);
            invent.RemoveAllItems();
        }
    }


    private void UpdateStoredItemsList()
    {
        InventorySystem currentInventory = inventory.GetComponent<InventorySystem>();
        storedItems = currentInventory.getItemList();
    }

    private void TransferItemsToInventory()
    {
        InventorySystem currentInventory = inventory.GetComponent<InventorySystem>();
        foreach (InventoryItem storedItem in storedItems)
        {
            currentInventory.addItemToInventory(storedItem.itemID, storedItem.itemValue);
        }
        currentInventory.UpdateStackableItems();
    }
}
