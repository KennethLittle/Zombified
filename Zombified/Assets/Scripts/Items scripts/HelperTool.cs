using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class HelperTool : MonoBehaviour
{

    public InventoryItem item;
    //GUI
    [SerializeField]
    public Image Background;
    [SerializeField]
    public Text NameText;
    [SerializeField]
    public Text DescText;

    //Tooltip Settings
    [SerializeField]
    public bool showToolBar;
    [SerializeField]
    private bool ToolBarCreated;
    [SerializeField]
    private int ToolBarWidth;
    [SerializeField]
    public int ToolBarHeight;
    [SerializeField]
    private bool ToolBarIcon;
    [SerializeField]
    private int ToolBarIconPosX;
    [SerializeField]
    private int ToolBarIconPosY;
    [SerializeField]
    private int ToolBarIconSize;
    [SerializeField]
    private bool showToolBarName;
    [SerializeField]
    private int ToolBarNamePosX;
    [SerializeField]
    private int ToolBarNamePosY;
    [SerializeField]
    private bool showToolBarDesc;
    [SerializeField]
    private int ToolBarDescPosX;
    [SerializeField]
    private int ToolBarDescPosY;
    [SerializeField]
    private int ToolBarDescSizeX;
    [SerializeField]
    private int ToolBarDescSizeY;

    //Tooltip Objects
    [SerializeField]
    private GameObject ToolBar;
    [SerializeField]
    private RectTransform ToolBarRectTransform;
    [SerializeField]
    private GameObject ToolBarTextName;
    [SerializeField]
    private GameObject ToolBarTextDesc;
    [SerializeField]
    private GameObject ToolBarImageIcon;

    void Start()
    {
        deactivateTooltip();
    }

#if UNITY_EDITOR
    [MenuItem("Master System/Create/HelperTool")]        //creating the menu item
    public static void menuItemCreateInventory()       //create the inventory at start
    {
        if (GameObject.FindGameObjectWithTag("HelperTool") == null)
        {
            GameObject toolTip = (GameObject)Instantiate(Resources.Load("Prefabs/Tooltip - Inventory") as GameObject);
            toolTip.GetComponent<RectTransform>().localPosition = new Vector3(Screen.width / 2, Screen.height / 2, 0);
            toolTip.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform);
            toolTip.AddComponent<HelperTool>().setImportantVariables();
        }
    }
#endif
    public void setImportantVariables()
    {
        ToolBarRectTransform = this.GetComponent<RectTransform>();

        ToolBarTextName = this.transform.GetChild(2).gameObject;
        ToolBarTextName.SetActive(false);
        ToolBarImageIcon = this.transform.GetChild(1).gameObject;
        ToolBarImageIcon.SetActive(false);
        ToolBarTextDesc = this.transform.GetChild(3).gameObject;
        ToolBarTextDesc.SetActive(false);

        ToolBarIconSize = 50;
        ToolBarWidth = 150;
        ToolBarHeight = 250;
        ToolBarDescSizeX = 100;
        ToolBarDescSizeY = 100;
    }

    public void setVariables()
    {
        Background = transform.GetChild(0).GetComponent<Image>();
        NameText = transform.GetChild(2).GetComponent<Text>();
        DescText = transform.GetChild(3).GetComponent<Text>();
    }

    public void activateTooltip()               //if you activate the tooltip through hovering over an item
    {
        ToolBarTextName.SetActive(true);
        ToolBarImageIcon.SetActive(true);
        ToolBarTextDesc.SetActive(true);
        transform.GetChild(0).gameObject.SetActive(true);          //Tooltip getting activated
        transform.GetChild(1).GetComponent<Image>().sprite = item.itemIcon;         //and the itemIcon...
        transform.GetChild(2).GetComponent<Text>().text = item.itemName;            //,itemName...
        transform.GetChild(3).GetComponent<Text>().text = item.itemDescription;            //and itemDesc is getting set        
    }

    public void deactivateTooltip()             //deactivating the tooltip after you went out of a slot
    {
        ToolBarTextName.SetActive(false);
        ToolBarImageIcon.SetActive(false);
        ToolBarTextDesc.SetActive(false);
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void updateTooltip()
    {
        if (!Application.isPlaying)
        {
            ToolBarRectTransform.sizeDelta = new Vector2(ToolBarWidth, ToolBarHeight);

            if (showToolBarName)
            {
                ToolBarTextName.gameObject.SetActive(true);
                this.transform.GetChild(2).GetComponent<RectTransform>().localPosition = new Vector3(ToolBarNamePosX, ToolBarNamePosY, 0);
            }
            else
            {
                this.transform.GetChild(2).gameObject.SetActive(false);
            }

            if (ToolBarIcon)
            {
                this.transform.GetChild(1).gameObject.SetActive(true);
                this.transform.GetChild(1).GetComponent<RectTransform>().localPosition = new Vector3(ToolBarIconPosX, ToolBarIconPosY, 0);
                this.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(ToolBarIconSize, ToolBarIconSize);
            }
            else
            {
                this.transform.GetChild(1).gameObject.SetActive(false);
            }

            if (showToolBarDesc)
            {
                this.transform.GetChild(3).gameObject.SetActive(true);
                this.transform.GetChild(3).GetComponent<RectTransform>().localPosition = new Vector3(ToolBarDescPosX, ToolBarDescPosY, 0);
                this.transform.GetChild(3).GetComponent<RectTransform>().sizeDelta = new Vector2(ToolBarDescSizeX, ToolBarDescSizeY);
            }
            else
            {
                this.transform.GetChild(3).gameObject.SetActive(false);
            }
        }
    }
}
