using UnityEngine;
using System.Collections;

[System.Serializable]
public class ItemStats
{

    public string attributeName;
    public int attributeValue;
    public ItemStats(string attributeName, int attributeValue)
    {
        this.attributeName = attributeName;
        this.attributeValue = attributeValue;
    }

    public ItemStats() { }

}
