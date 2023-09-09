using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

[CreateAssetMenu(fileName = "New MedPack Object", menuName = "Inventory System/Items/MedPack")]
public class medPackStats : BaseItemStats
{
    public int healAmount;
    
}
