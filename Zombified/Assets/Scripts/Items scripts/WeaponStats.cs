using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "New Weapon Object", menuName = "Inventory System/Items/Weapon")]
public class WeaponStats : ScriptableObject
{
    public float shootRate;
    public int shootDamage;
    public float shootDist;
    public int ammoCur;
    public int ammoMax;

    [SerializeField] public AudioClip[] audioShoot;
    [SerializeField] [Range(0, 1)] public float audioShootVol;
    [SerializeField] public AudioClip[] audioShootCasing;
    [SerializeField] [Range(0, 1)] public float audioShootCasingVol;
    [SerializeField] public AudioClip[] audioGunReload;
    [SerializeField] [Range(0, 1)] public float audioGunReloadVol;

    public Sprite icon;

    public GameObject model;
    public ParticleSystem hitEffect;

}
