using UnityEngine;

public class ItemBehavior : MonoBehaviour
{
    public BaseItemStats itemStats;
    public WeaponStats weaponStats;

    private float shootRate;
    private int shootDamage;
    private float shootDist;
    private int ammoCur;
    private int ammoMax;

    private AudioClip[] audioShoot;
    private float audioShootVol;
    private AudioClip[] audioShootCasing;
    private float audioShootCasingVol;
    private AudioClip[] audioGunReload;
    private float audioGunReloadVol;

    private ParticleSystem hitEffect;

    private void Start()
    {
        if (itemStats != null)
            gameObject.name = itemStats.itemName;

        // For weapon-specific properties, you can cast and check:
        if (itemStats is WeaponStats)
        {
            WeaponStats weaponStats = itemStats as WeaponStats;

            shootRate = weaponStats.shootRate;
            shootDamage = weaponStats.shootDamage;
            shootDist = weaponStats.shootDist;
            ammoCur = weaponStats.ammoCur;
            ammoMax = weaponStats.ammoMax;

            audioShoot = weaponStats.audioShoot;
            audioShootVol = weaponStats.audioShootVol;
            audioShootCasing = weaponStats.audioShootCasing;
            audioShootCasingVol = weaponStats.audioShootCasingVol;
            audioGunReload = weaponStats.audioGunReload;
            audioGunReloadVol = weaponStats.audioGunReloadVol;

            hitEffect = weaponStats.hitEffect;

            // ... and so on for other properties within WeaponStats
        }
        else if (itemStats is ammoBoxStats)
        {
            ammoBoxStats ammoBox = itemStats as ammoBoxStats;

            // Here you can reference and use the specific properties of AmmoBoxStats
            // For example: 
            int ammoAmount = ammoBox.ammoAmount;

            // ... and so on for other properties within AmmoBoxStats
        }
        else if (itemStats is medPackStats)
        {
            medPackStats medPack = itemStats as medPackStats;

            // Here you can reference and use the specific properties of MedPackStats
            // For example:
            int healAmount = medPack.healAmount;

            // ... and so on for other properties within MedPackStats
        }
    }
}
