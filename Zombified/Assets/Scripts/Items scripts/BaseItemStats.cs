using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseItemStats : ScriptableObject
{
    public string itemName = "New Item";
    public Sprite icon;
    public GameObject modelPrefab;
}
