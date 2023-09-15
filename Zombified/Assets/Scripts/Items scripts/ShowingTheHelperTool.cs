using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;

public class ShowingTheHelperTool : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public HelperTool Helper;                    // Helper script
    public GameObject HelperToolGameObject;      // HelperTool GameObject
    public RectTransform CanvasRectTransform;    // The Inventory Background
    public RectTransform HelperToolRectTransform; // the HelperTool RectTransform
    private InventoryItem item;
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindGameObjectWithTag("HelperTool") != null)
        {
            Helper = GameObject.FindGameObjectWithTag("HelperTool").GetComponent<HelperTool>();
            HelperToolGameObject = GameObject.FindGameObjectWithTag("HelperTool");
            HelperToolRectTransform = HelperToolGameObject.GetComponent<RectTransform>() as RectTransform;
        }
        CanvasRectTransform = GameObject.FindGameObjectWithTag("Canvas").GetComponent<RectTransform>() as RectTransform;
    }

    public void OnPointerEnter(PointerEventData data)
    {
        if (Helper != null)
        {
            item = GetComponent<ItemWithObject>().item;                   //we get the item
            Helper.item = item;                                        //set the item in the tooltip
            Helper.activateTooltip();                                  //set all informations of the item in the tooltip
            if (CanvasRectTransform == null)
                return;


            Vector3[] slotCorners = new Vector3[4];                     //get the corners of the slot
            GetComponent<RectTransform>().GetWorldCorners(slotCorners); //get the corners of the slot                

            Vector2 localPointerPosition;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(CanvasRectTransform, slotCorners[3], data.pressEventCamera, out localPointerPosition))   // and set the localposition of the tooltip...
            {
                if (transform.parent.parent.parent.GetComponent<ToolBar>() == null)
                    HelperToolRectTransform.localPosition = localPointerPosition;          //at the right bottom side of the slot
                else
                    HelperToolRectTransform.localPosition = new Vector3(localPointerPosition.x, localPointerPosition.y + Helper.ToolBarHeight);
            }

        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

}
