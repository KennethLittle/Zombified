using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Database", menuName = "AttributeList")]
public class ItemAttributeList : ScriptableObject
{
    [SerializeField]
    public List<ItemStats> itemStatsList = new List<ItemStats>();

}
