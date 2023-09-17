using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventoryDesign : MonoBehaviour
{
    // UIDesign
    [SerializeField]
    private Image slot;
    [SerializeField]
    private Image activeSlotDesign;
    [SerializeField]
    private Image inventoryBackground;
    [SerializeField]
    private bool displayInventoryCloseButton;
    [SerializeField]
    private Image closeButtonImage;
    [SerializeField]
    private RectTransform closeButtonPosition;
    [SerializeField]
    private int closeButtonPosX;
    [SerializeField]
    private int closeButtonPosY;
    [SerializeField]
    private string inventoryHeader;
    [SerializeField]
    private Text headerText;
    [SerializeField]
    private int headerPosX;
    [SerializeField]
    private int headerPosY;
    [SerializeField]
    private int panelWidth;
    [SerializeField]
    private int panelHeight;

    public void ApplyDesignSettings()
    {
        headerPosX = (int)transform.GetChild(0).GetComponent<RectTransform>().localPosition.x;
        headerPosY = (int)transform.GetChild(0).GetComponent<RectTransform>().localPosition.y;
        panelWidth = (int)GetComponent<RectTransform>().sizeDelta.x;
        panelHeight = (int)GetComponent<RectTransform>().sizeDelta.y;
        inventoryHeader = transform.GetChild(0).GetComponent<Text>().text;
        headerText = transform.GetChild(0).GetComponent<Text>();

        if (GetComponent<ToolBar>() == null)
        {
            closeButtonPosition = transform.GetChild(2).GetComponent<RectTransform>();
            closeButtonImage = transform.GetChild(2).GetComponent<Image>();
        }

        inventoryBackground = GetComponent<Image>();
        activeSlotDesign = transform.GetChild(1).GetChild(0).GetComponent<Image>();
        slot = activeSlotDesign;
        slot.sprite = activeSlotDesign.sprite;
        slot.color = activeSlotDesign.color;
        slot.material = activeSlotDesign.material;
        slot.type = activeSlotDesign.type;
        slot.fillCenter = activeSlotDesign.fillCenter;
    }

    public void UpdateDesign()
    {
        transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector3(headerPosX, headerPosY, 0);
        transform.GetChild(0).GetComponent<Text>().text = inventoryHeader;
    }

    public void UpdateCloseButton()
    {
        GameObject closeButton = transform.GetChild(2).gameObject;
        if (displayInventoryCloseButton)
        {
            closeButton.SetActive(displayInventoryCloseButton);
            closeButtonPosition.localPosition = new Vector3(closeButtonPosX, closeButtonPosY, 0);
        }
        else
        {
            closeButton.SetActive(displayInventoryCloseButton);
        }
    }

    public void RefreshAllSpaces()
    {
        Image space = null;
        for (int i = 0; i < transform.GetChild(1).childCount; i++)
        {
            space = transform.GetChild(1).GetChild(i).GetComponent<Image>();
            space.sprite = slot.sprite;
            space.color =  slot.color;
            space.material = slot.material;
            space.type = slot.type;
            space.fillCenter = slot.fillCenter;
        }
    }
}
