using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static InventoryItem;

public class playerController : MonoBehaviour, IDamage
{
    public PlayerStat playerStat;
    public Transform weaponSlot;
    GameObject inventory;
    GameObject craftSystem;
    GameObject characterSystem;


    public PlayerEquipment playerInventoryUI;

    [Header("----- Character -----")]
    [SerializeField] CharacterController controller;
    [SerializeField] Animator anim;


    [Header("----- Player Gun Stats -----")]
    [SerializeField] public List<GameObject> equippedWeapons = new List<GameObject>(); // List of instantiated weapon game objects
    public int currentWeaponIndex = 0;

    private float originalPlayerSpeed;
    private bool groundedPlayer;
    private Vector3 move;
    private Vector3 playerVelocity;
    private int jumpCount;
    private bool isSprinting;
    private bool isShooting;
    private bool footstepsIsPlaying;
    private bool lowHealthIsPlaying;
    private float lastJumpTime = 0f;
    private float jumpCooldown = 3f;
    private float audioLHVolOrig;
    private float walkVolume;
 
    private void Start()
    {
        
        originalPlayerSpeed = playerStat.playerSpeed;
        playerStat.HP = playerStat.HPMax;
        playerStat.currentStamina = playerStat.stamina;
        
        //this is for the changing the rate and volume of footsteps
        foreach (var sound in AudioManager.instance.PlayerSounds)
        {
            if ( sound.name == "Footsteps")
            {
                walkVolume = sound.volume;
            }
        }
        audioLHVolOrig = walkVolume;
        spawnPlayer();
    }

    void Update()
    {
        movement();
        sprint();
        lowHealthSFX();
        if (weaponSlot.transform.childCount > 0 && Input.GetButton("Shoot") && !isShooting)
        {
            StartCoroutine(shoot());
        }
        if (Input.GetButtonDown("SwitchWeapons"))
        {
            SwitchToNextWeapon();
        }

    }

    public void ResetToDefaults()
    {
        playerStat.HP = playerStat.HPMax;
        playerStat.stamina = playerStat.currentStamina;
    }

    public void SetInitialStats(int level, int hp, int stamina)
    {
        playerStat.Level = level;
        playerStat.HP = hp;
        playerStat.stamina = stamina;
    }

    public void takeDamage(int amount)
    {
        // Plays damaged audio sfx - Plays a random damaged sfx from the range audioDamage at a volume defined by audioDamageVol 
        playerStat.HP -= amount;
        StartCoroutine(UIManager.Instance.PlayerFlashDamage());
        updatePlayerUI();
        
        if (playerStat.HP <= 0)
        {
            anim.SetTrigger("IsDead");
            gameManager.instance.Defeat();
            gameManager.instance.MarkRunEnd();
        }

        AudioManager.instance.PlaySound("Take Damage", AudioManager.instance.PlayerSounds);
    }

    void lowHealthSFX()
    {
        if (!lowHealthIsPlaying && playerStat.HP <= (playerStat.HPMax * 0.3))
        {
            StartCoroutine((playLowHealth()));
        }
    }

    IEnumerator playLowHealth()
    {
        lowHealthIsPlaying = true;
        // Plays low health audio sfx - Plays a random footsteps sfx from the range audioLowHealth at a volume defined by audioLowHealthVol

        if (playerStat.HP <= (playerStat.HPMax * 0.3) && playerStat.HP > (playerStat.HPMax * 0.2))
        {
            foreach (var sound in AudioManager.instance.PlayerSounds)
            {
                if (sound.name == "Low Health")
                {
                    sound.volume = audioLHVolOrig + 0.2f;
                }
            }
            yield return new WaitForSeconds(2.0f);
        }
        else if (playerStat.HP <= (playerStat.HPMax * 0.2) && playerStat.HP > (playerStat.HPMax * 0.1))
        {
            foreach (var sound in AudioManager.instance.PlayerSounds)
            {
                if (sound.name == "Low Health")
                {
                    sound.volume = audioLHVolOrig + 0.4f;
                }
            }
            yield return new WaitForSeconds(1.5f);
        }
        else if (playerStat.HP <= (playerStat.HPMax * 0.1) && playerStat.HPMax > 0)
        {
            foreach (var sound in AudioManager.instance.PlayerSounds)
            {
                if (sound.name == "Low Health")
                {
                    sound.volume = audioLHVolOrig + 0.6f;
                }
            }
            yield return new WaitForSeconds(1.0f);
        }
        AudioManager.instance.PlaySound("LowHealth", AudioManager.instance.PlayerSounds);
        lowHealthIsPlaying = false;
    }

    void movement()
    {
        groundedPlayer = controller.isGrounded;

        HandleGroundedState();

        HandlePlayerInput();

        ApplyGravity();

        // Combine the movement from input and the velocity due to external forces.
        controller.Move((move + playerVelocity) * Time.deltaTime);

        // Reset the IsJumping animation parameter if the player is grounded
        if (groundedPlayer)
        {
            anim.SetBool("IsJumping", false);
        }
    }

    void HandleGroundedState()
    {
        if (groundedPlayer)
        {
            playerVelocity.y = 0f; // Ensures the player does not accumulate downward velocity when grounded.
            jumpCount = 0;

            if (!footstepsIsPlaying && move.normalized.magnitude > 0.5f && playerStat.HP > 0)
            {
                StartCoroutine(playFootsteps());
            }
        }
    }

    void HandlePlayerInput()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        move = (horizontalInput * transform.right) + (verticalInput * transform.forward);

        float effectivePlayerSpeed = isSprinting ? originalPlayerSpeed * playerStat.sprintMod : originalPlayerSpeed;

        if (move != Vector3.zero)
        {
            if (groundedPlayer)
            {
                if (isSprinting && playerStat.currentStamina > 0)
                {
                    move *= effectivePlayerSpeed * playerStat.playerSpeed;
                    anim.SetBool("isWalking", false);
                    anim.SetBool("isRunning", isSprinting);
                }
                else
                {
                    isSprinting = false;
                    anim.SetBool("isWalking", true);
                    anim.SetBool("isRunning", isSprinting);
                    move *= playerStat.playerSpeed;
                }
            }
            else
            {
                // This preserves momentum while airborne.
                move.x = playerVelocity.x;
                move.z = playerVelocity.z;
            }

        }
        else
        {
            anim.SetBool("isWalking", false);
            anim.SetBool("isRunning", false);

        }

        // Handle Jumping
        if (Input.GetButtonDown("Jump") && jumpCount < playerStat.jumpMax && Time.time - lastJumpTime > jumpCooldown)
        {
            lastJumpTime = Time.time;
            AudioManager.instance.PlaySound("Jump", AudioManager.instance.PlayerSounds);

            playerVelocity.y += playerStat.jumpHeight;
            jumpCount++;
        }
    }

    void ApplyGravity()
    {
        playerVelocity.y += playerStat.gravityValue * Time.deltaTime;
    }

    //Play footsteps sfx at a rate defined by footstepsRate
    IEnumerator playFootsteps()
    {
        footstepsIsPlaying = true;
        // Plays footsteps audio sfx - Plays a random footsteps sfx from the range audioFootsteps at a volume defined by audioFootstepsVol
        
        AudioManager.instance.PlaySound("Footsteps", AudioManager.instance.PlayerSounds);
        
        if (!isSprinting)
        {
            foreach (var sound in AudioManager.instance.PlayerSounds)
            {
                if (sound.name == "Footsteps")
                {
                    yield return new WaitForSeconds(sound.rate);
                }
            }
        }
        else
        {
            foreach (var sound in AudioManager.instance.PlayerSounds)
            {
                if (sound.name == "Footsteps")
                {
                    yield return new WaitForSeconds(sound.rate / 2);
                }
            }
        }
        footstepsIsPlaying = false;
    }

    void sprint()
    {
        if (Input.GetButtonDown("Sprint") && playerStat.currentStamina > 0) // Add stamina check here
        {
            isSprinting = true;
        }
        else if (Input.GetButtonUp("Sprint") || playerStat.currentStamina <= 0) // Add stamina check here
        {
            isSprinting = false;
        }
        if (isSprinting)
        {
            ConsumeStamina(playerStat.staminaConsumptionRate * Time.deltaTime);
        }
        else
        {
            RegenerateStamina(playerStat.staminaRegenerationRate * Time.deltaTime);
        }

    }

    public void ConsumeStamina(float amount)
    {
        playerStat.currentStamina = Mathf.Clamp(playerStat.currentStamina - amount, 0f, playerStat.stamina);

        if (playerStat.currentStamina <= 0f)
        {
            isSprinting = false;
        }

        updatePlayerUI();
    }

    public void RegenerateStamina(float amount)
    {
        playerStat.currentStamina = Mathf.Clamp(playerStat.currentStamina + amount, 0f, playerStat.stamina);
       
        updatePlayerUI();
    }

    IEnumerator shoot()
    {
        if (weaponSlot.transform.childCount == 0)
        {
            Debug.Log("No weapon in the slot.");
            yield break;
        }

        ItemBehavior itemBehavior = weaponSlot.transform.GetChild(0).GetComponent<ItemBehavior>();
        if (itemBehavior == null || itemBehavior.weaponStats == null)
        {
            Debug.Log("ItemBehavior or WeaponStats not found.");
            yield break;
        }

        WeaponStats weapon = itemBehavior.weaponStats; // <-- Here's where we set the weapon stats

        if (weapon.ammoCur > 0)
        {
            isShooting = true;

            weapon.ammoCur--;
            updatePlayerUI();

            // Plays gunshot audio sfx
            //audioSFX.PlayOneShot(weapon.audioShoot[Random.Range(0, weapon.audioShoot.Length)], weapon.audioShootVol);

            // Plays gunshot casing audio sfx
            //audioSFX.PlayOneShot(weapon.audioShootCasing[Random.Range(0, weapon.audioShootCasing.Length)], weapon.audioShootCasingVol);

            // Shoot code
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, weapon.shootDist))
            {
                IDamage damageable = hit.collider.GetComponent<IDamage>();

                if (damageable != null)
                {
                    damageable.takeDamage(weapon.shootDamage);
                }
            }

            yield return new WaitForSeconds(weapon.shootRate);
            isShooting = false;
        }
    }

    WeaponStats GetCurrentWeaponStats()
    {
        if (currentWeaponIndex >= 0 && currentWeaponIndex < equippedWeapons.Count)
        {
            ItemBehavior itemBehavior = equippedWeapons[currentWeaponIndex].GetComponent<ItemBehavior>();
            return itemBehavior?.itemStats as WeaponStats;
        }
        return null;
    }

    private void SwitchToNextWeapon()
    {
        if (equippedWeapons.Count > 0)
        {
            // Deactivate the current weapon
            if (currentWeaponIndex >= 0 && currentWeaponIndex < equippedWeapons.Count)
            {
                equippedWeapons[currentWeaponIndex].SetActive(false);
            }

            // Increment the weapon index
            currentWeaponIndex = (currentWeaponIndex + 1) % equippedWeapons.Count;

            // Activate the new weapon
            equippedWeapons[currentWeaponIndex].SetActive(true);

            // Update the player UI
            updatePlayerUI();
        }
    }

    public void spawnPlayer()
    {
        controller.enabled = false;
        transform.position = PlayerManager.instance.playerSpawnPos.transform.position;
        controller.enabled = true;
        playerStat.HP = playerStat.HPMax;
        foreach (var sound in AudioManager.instance.PlayerSounds)
        {
            if (sound.name == "LowHealth")
            {
                sound.volume = audioLHVolOrig;
            }
        }
        audioLHVolOrig = walkVolume;
        updatePlayerUI();
    }

    public void IncreaseMaxHP(int amount)
    {
        if (!gameManager.instance.isInRun)
        {
             
            playerStat.HPMax += amount;
            playerStat.HP += amount;
            updatePlayerUI();
        }
    }

    public void IncreaseMaxStamina(int amount)
    {
        if (!gameManager.instance.isInRun)
        {
            
            playerStat.stamina += amount;
            playerStat.currentStamina += amount;
            updatePlayerUI();
        }
    }

    public void updatePlayerUI()
    {
        UIManager.Instance.playerHPBar.fillAmount = (float)playerStat.HP / playerStat.HPMax;
        UIManager.Instance.staminaBar.fillAmount = playerStat.currentStamina / playerStat.stamina;

        WeaponStats currentWeapon = GetCurrentWeaponStats();
        if (currentWeapon != null)
        {
            UIManager.Instance.ammoCur.text = currentWeapon.ammoCur.ToString("F0");
            UIManager.Instance.ammoMax.text = currentWeapon.ammoMax.ToString("F0");
            UIManager.Instance.weaponIcon.sprite = currentWeapon.icon;
        }

    }

    public void EquipWeapon(BaseItemStats weapon)
    {
        if (weaponSlot.transform.childCount > 0)
        {
            foreach (Transform child in weaponSlot.transform)
            {
                Destroy(child.gameObject);
            }
        }

        if (weapon.itemType == ItemType.Weapon)
        {
            GameObject equippedWeaponGO = Instantiate(weapon.modelPrefab, weaponSlot.transform.position, Quaternion.identity, weaponSlot.transform);
            weaponSlot.GetComponent<MeshFilter>().sharedMesh = weapon.modelPrefab.GetComponent<MeshFilter>().sharedMesh;
            weaponSlot.GetComponent<MeshRenderer>().sharedMaterial = weapon.modelPrefab.GetComponent<MeshRenderer>().sharedMaterial;
            ItemBehavior weaponBehavior = equippedWeaponGO.AddComponent<ItemBehavior>();
            weaponBehavior.itemStats = weapon;

            equippedWeapons.Add(equippedWeaponGO);
        }
    }


}
