using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LocationData", menuName = "Game/LocationData")]
public class LocationData : ScriptableObject
{
    public enum Location
    {
        HomeBase,
        AlphaStation
    }

    public Location currentLocation;

}
