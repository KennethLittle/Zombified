using UnityEngine;

public class ItemHandler : MonoBehaviour
{
    [SerializeField]
    private BaseItemStats itemStats;

    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        if (itemStats != null)
        {
            InitializeItem();
        }
        else
        {
            Debug.LogError("No item stats assigned to: " + gameObject.name);
        }
    }

    void InitializeItem()
    {
        // Set sprite
        if (spriteRenderer != null && itemStats.icon != null)
        {
            spriteRenderer.sprite = itemStats.icon;
        }

        // Check and initialize based on type
        if (itemStats is ammoBoxStats)
        {
            // If you have specific logic to handle during initialization for ammoBoxStats, handle it here
        }
        else if (itemStats is WeaponStats weapon)
        {
            InitializeWeapon(weapon);
        }
        else if (itemStats is medPackStats)
        {
            // If you have specific logic to handle during initialization for medPackStats, handle it here
        }
    }

    void InitializeWeapon(WeaponStats weapon)
    {
        // Handle audio for weapon
        if (audioSource != null)
        {
            if (weapon.audioShoot.Length > 0)
            {
                audioSource.clip = weapon.audioShoot[0];
                audioSource.volume = weapon.audioShootVol;
            }
        }
        // No need to set properties like damage, shootRate, etc. here since they are in the WeaponStats
        // When you need them, you can access them directly from the WeaponStats object attached to this handler.
    }

    public BaseItemStats GetItemStats()
    {
        return itemStats;
    }
}



