using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
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

    public GameObject model;
    public ParticleSystem hitEffect;
}
