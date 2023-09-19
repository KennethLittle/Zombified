using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class WeaponDetails
{
    public GameObject projectilePrefab;  // Prefab for the projectile (e.g. bullet, arrow, etc.)
    public float fireRate; // Number of times weapon can be fired in a second
    public float range; // The range of the weapon if it's melee
    public int ammoCurrent;
    public int ammoMax;
    public int ammoAdditional;
}