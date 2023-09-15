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
    private GameObject inventoryPanel;

    [SerializeField]
    private List<InventoryItem> storedItems = new List<InventoryItem>();

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
    private InventorySystem inventory;

    private GameObject player;

    private static Image interactionTimerImage;
    private static GameObject interactionTimerObject;

    private bool closeInventoryFlag;

    private bool isInteractingWithStorage;

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
        inventory = inventoryPanel.GetComponent<InventorySystem>();
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

        if (GameObject.FindGameObjectWithTag("Timer") != null)
        {
            interactionTimerImage = GameObject.FindGameObjectWithTag("Timer").GetComponent<Image>();
            interactionTimerObject = GameObject.FindGameObjectWithTag("Timer");
            interactionTimerObject.SetActive(false);
        }
        if (GameObject.FindGameObjectWithTag("HelperTool") != null)
            itemHelper = GameObject.FindGameObjectWithTag("HelperTool").GetComponent<HelperTool>();
    }

    public void InitializeImportantVariables()
    {
        if (itemDatabase == null)
            itemDatabase = (DataBaseForItems)Resources.Load("ItemDatabase");
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(this.gameObject.transform.position, player.transform.position);

        if (displayInteractionTimer && interactionTimerImage != null)
        {
            interactionTimerObject.SetActive(true);
            float fillPercentage = (Time.time - interactionStart) / interactionDelay;
            interactionTimerImage.fillAmount = fillPercentage;
        }

        if (distanceToPlayer <= interactionDistance && Input.GetKeyDown(inputManager.StorageKeyCode))
        {
            isInteractingWithStorage = !isInteractingWithStorage;
            StartCoroutine(OpenCloseInventoryWithDelay());
        }

        if (distanceToPlayer > interactionDistance && isInteractingWithStorage)
        {
            isInteractingWithStorage = false;
            if (inventoryPanel.activeSelf)
            {
                storedItems.Clear();
                UpdateStoredItemsList();
                inventoryPanel.SetActive(false);
                inventory.RemoveAllItems();
            }
            itemHelper.deactivateHelperTool();
            interactionTimerImage.fillAmount = 0;
            interactionTimerObject.SetActive(false);
            displayInteractionTimer = false;
        }
    }

    IEnumerator OpenCloseInventoryWithDelay()
    {
        if (isInteractingWithStorage)
        {
            interactionStart = Time.time;
            displayInteractionTimer = true;
            yield return new WaitForSeconds(interactionDelay);
            if (isInteractingWithStorage)
            {
                inventory.ItemsInInventory.Clear();
                inventoryPanel.SetActive(true);
                TransferItemsToInventory();
                displayInteractionTimer = false;
                interactionTimerObject.SetActive(false);
            }
        }
        else
        {
            storedItems.Clear();
            UpdateStoredItemsList();
            inventoryPanel.SetActive(false);
            inventory.RemoveAllItems();
            itemHelper.deactivateHelperTool();
        }
    }

    private void UpdateStoredItemsList()
    {
        InventorySystem currentInventory = inventoryPanel.GetComponent<InventorySystem>();
        storedItems = currentInventory.getItemList();
    }

    private void TransferItemsToInventory()
    {
        InventorySystem currentInventory = inventoryPanel.GetComponent<InventorySystem>();
        foreach (InventoryItem storedItem in storedItems)
        {
            currentInventory.addItemToInventory(storedItem.itemID, storedItem.itemValue);
        }
        currentInventory.UpdateStackableItems();
    }
}
