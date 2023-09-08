using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;

public class Inventoryitem : MonoBehaviour
{
    public enum ItemType
    {
        General,
        PrimaryWeapon,
        SecondaryWeapon
    }

    public ItemType itemType;

}
