using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WeaponAudio : ScriptableObject
{
    [SerializeField] AudioClip[] audioShoot;
    [SerializeField] [Range(0, 1)] float audioShootVol;
    [SerializeField] AudioClip[] audioShootCasing;
    [SerializeField] [Range(0, 1)] float audioShootCasingVol;
    [SerializeField] AudioClip[] audioGunReload;
    [SerializeField] [Range(0, 1)] float audioGunReloadVol;
}
