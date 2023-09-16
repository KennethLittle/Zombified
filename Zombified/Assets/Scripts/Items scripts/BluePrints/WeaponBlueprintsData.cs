using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBlueprintsData : ScriptableObject
{
    [SerializeField]
    public List<WeaponBluePrint> weapons = new List<WeaponBluePrint>();
}
