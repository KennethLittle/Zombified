using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor.Recorder;
using UnityEngine;
using UnityEngine.EventSystems;

public class playerController : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    public PlayerStat playerStat;
    public Transform weaponSlot;
    public InventoryItem currentEquippedItem;
    public PlayerSounds PlayerSounds;
    [SerializeField] CharacterController controller;
    [SerializeField] Animator anim;
    public GameObject source;

    [Header("----- Player Gun Stats -----")]
    [SerializeField] public List<GameObject> equippedWeapons = new List<GameObject>();
    public int currentWeaponIndex = 0;



    private float originalPlayerSpeed;
    private bool groundedPlayer;
    private Vector3 move;
    private Vector3 playerVelocity;
    private int jumpCount;
    private bool isSprinting;
    private bool isShooting;
    private bool isReloading = false;
    private float reloadDuration = 2.0f;
    private bool lowHealthIsPlaying;
    private bool walkVolume;
    private bool footstepsIsPlaying;
    private float lastJumpTime = 0f;
    private float jumpCooldown = 1f;
    private bool isJumping;
    private bool canPlayDamageSound = true;


    private void Start()
    {
        originalPlayerSpeed = playerStat.playerSpeed;
        playerStat.HP = playerStat.HPMax;
        playerStat.currentStamina = playerStat.stamina;
        isSprinting = false;

    }

    void Update()
    {
        movement();
        sprint();
        PlayLowHealth();
        if (PlayerEquipment.Instance.equippedWeapon != null && PlayerEquipment.Instance.equippedWeapon.weaponDetails.ammoCurrent > 0 && Input.GetMouseButton(0) && !isShooting)
        {
            StartCoroutine(Shooting());
        }
        

        if (Input.GetKeyDown(KeyCode.R) && !isReloading)
        {
            StartCoroutine(Reload());
            updatePlayerUI();
        }
    }

    public void EquipItem(InventoryItem item)
    {
        currentEquippedItem = item;

        // If the equipped item is a weapon, set the weapon's details in the PlayerEquipment script.
        if (item.itemType == ItemType.Weapon)
        {
            PlayerEquipment.Instance.EquipWeapon(item);
        }
    }

    public void UnEquipItem(InventoryItem item)
    {
        if (currentEquippedItem != null)
        {
            if (currentEquippedItem.itemType == ItemType.Weapon)
            {
                PlayerEquipment.Instance.UnEquipWeapon(item);
            }
            currentEquippedItem = null;
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

    IEnumerator Shooting()
    {
        WeaponDetails weapon = PlayerEquipment.Instance.equippedWeapon.weaponDetails;

        while (weapon != null && weapon.ammoCurrent > 0 && Input.GetButton("Shooting"))
        {
            isShooting = true;
            anim.SetTrigger("isShooting");
            float fireRate = weapon.fireRate;
            float shootDist = weapon.range;
            int damage = weapon.damage;

            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDist))
            {
                PlayerSounds.ShootEmote();
                IDamage damageable = hit.collider.GetComponent<IDamage>();
                if (damageable != null)
                {
                    damageable.takeDamage(damage, source);
                }
            }
            //if (weapon.projectilePrefab != null)
            //{
            //    Instantiate(weapon.projectilePrefab, weaponSlot.position, weaponSlot.rotation);
            //}

            weapon.ammoCurrent--;
            updatePlayerUI();


            float fireInterval = 1.0f / weapon.fireRate;
            yield return new WaitForSeconds(fireInterval);
            isShooting = false;

        }
    }

    IEnumerator Reload()
    {
        if (PlayerEquipment.Instance.equippedWeapon != null)
        {
            WeaponDetails weapon = PlayerEquipment.Instance.equippedWeapon.weaponDetails;

            // If there's no need to reload, exit early
            if (weapon.ammoCurrent >= weapon.ammoMax || weapon.ammoAdditional <= 0)
                yield break;

            isReloading = true;
            anim.SetTrigger("isReloading");
            // Wait for the reload duration
            PlayerSounds.ReloadEmote();
            yield return new WaitForSeconds(reloadDuration);

            int ammoNeeded = weapon.ammoMax - weapon.ammoCurrent; // Calculate the amount needed to fill the current ammo
            int ammoToTransfer = Mathf.Min(ammoNeeded, weapon.ammoAdditional); // Determine the amount to transfer, considering the backup ammo

            weapon.ammoCurrent += ammoToTransfer; // Add the transferred ammo to the current ammo
            weapon.ammoAdditional -= ammoToTransfer; // Subtract the transferred ammo from the backup ammo

            isReloading = false;
        }
    }

    public void takeDamage(int amount, GameObject source)
    {
        anim.SetTrigger("isHit");
        playerStat.HP -= amount;
        updatePlayerUI();
        Debug.Log("Damage from: " + source.name + ", Amount: " + amount);
        StartCoroutine(UIManager.Instance.PlayerFlashDamage());
        if (playerStat.HP <= 0)
        {
            UIManager.Instance.YouDeadSucka();
            anim.SetTrigger("IsDead");
        }
        TakeDamageSound();
    }
    public async void TakeDamageSound()
    {
        if (canPlayDamageSound)
        {
            PlayerSounds.TakeDamageEmote();
            canPlayDamageSound = false;

            int number = Random.Range(0, 30);
            await Task.Delay(number);
            canPlayDamageSound = true;
        }
        
    }

    public void updatePlayerUI()
    {
        UIManager.Instance.playerHPBar.fillAmount = (float)PlayerStat.Instance.HP / PlayerStat.Instance.HPMax;
        UIManager.Instance.staminaBar.fillAmount = (float)PlayerStat.Instance.currentStamina / PlayerStat.Instance.stamina;

        if (PlayerEquipment.Instance.equippedWeapon != null)
        {
            WeaponDetails weapon = PlayerEquipment.Instance.equippedWeapon.weaponDetails;

            UIManager.Instance.ammoCur.text = weapon.ammoCurrent.ToString();
            UIManager.Instance.ammoMax.text = weapon.ammoMax.ToString();
            UIManager.Instance.ammoBoxAmount.text = weapon.ammoAdditional.ToString();
        }
    }

    void PlayLowHealth()
    {
        if (playerStat.HP < 20 && playerStat.HP > 0)
        {
            PlayerSounds.LowHealthEmote();
        }
        else
        {
            PlayerSounds.DamageSource.Stop();
        }
    }
    IEnumerator playLowHealth()
    {
        lowHealthIsPlaying = true;
        // Plays low health audio sfx - Plays a random footsteps sfx from the range audioLowHealth at a volume defined by audioLowHealthVol

        if (playerStat.HP <= (playerStat.HPMax * 0.3) && playerStat.HP > (playerStat.HPMax * 0.2))
        {
            //foreach (var sound in AudioManager.instance.PlayerSounds)
            //{
            //    if (sound.name == "Low Health")
            //    {
            //        sound.volume = audioLHVolOrig + 0.2f;
            //    }
            //}
            yield return new WaitForSeconds(2.0f);
        }
        else if (playerStat.HP <= (playerStat.HPMax * 0.2) && playerStat.HP > (playerStat.HPMax * 0.1))
        {
            //foreach (var sound in AudioManager.instance.PlayerSounds)
            //{
            //    if (sound.name == "Low Health")
            //    {
            //        sound.volume = audioLHVolOrig + 0.4f;
            //    }
            //}
            yield return new WaitForSeconds(1.5f);
        }
        else if (playerStat.HP <= (playerStat.HPMax * 0.1) && playerStat.HPMax > 0)
        {
            //foreach (var sound in AudioManager.instance.PlayerSounds)
            //{
            //    if (sound.name == "Low Health")
            //    {
            //        sound.volume = audioLHVolOrig + 0.6f;
            //    }
            //}
            yield return new WaitForSeconds(1.0f);
        }
        //AudioManager.instance.PlaySound("LowHealth", AudioManager.instance.PlayerSounds);
        lowHealthIsPlaying = false;

    }

    void movement()
    {
        groundedPlayer = controller.isGrounded;

        HandleGroundedState();
        HandlePlayerInput();
        ApplyGravity();
        controller.Move((move + playerVelocity) * Time.deltaTime);


    }

    void HandleGroundedState()
    {

        if (groundedPlayer)
        {
            playerVelocity.y = 0f;
            jumpCount = 0;
              if (isJumping)
            {
                PlayerSounds.LandEmote();
                isJumping = false;
            }
        }        
        //Handle FootStepSFX      
    }

    void HandlePlayerInput()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        move = (horizontalInput * transform.right) + (verticalInput * transform.forward);
        float effectivePlayerSpeed = isSprinting ? originalPlayerSpeed * playerStat.sprintMod : originalPlayerSpeed;
        move.Normalize(); // this keeps the player speed from doubling when moving diaganolly.

        if (move != Vector3.zero)
        {
            if (groundedPlayer)
            {
                
                anim.SetBool("isWalking", !isSprinting);
                anim.SetBool("isRunning", isSprinting);
            }
            move *= (isSprinting && playerStat.currentStamina > 0) ? effectivePlayerSpeed * playerStat.playerSpeed : playerStat.playerSpeed;

            if (move.normalized.magnitude > 0f && !isJumping)
            {
                //playerVelocity = move * playerStat.playerSpeed;
                PlayerSounds.PlayFootstep(move * 1.85f);
                if (isSprinting)
                {
                    PlayerSounds.PlayFootstep(move * playerStat.sprintMod * 0.00005f);
                    //the whole .00005 thing does nothing. Don't know why the thing won't just do move * sprintmod.
                }
            }

        }
        else
        {
            anim.SetBool("isWalking", false);
            anim.SetBool("isRunning", false);
        }

        if (Input.GetButtonDown("Jump") && jumpCount < playerStat.jumpMax && Time.time - lastJumpTime > jumpCooldown)
        {
            lastJumpTime = Time.time;
            PlayerSounds.JumpEmote();
            playerVelocity.y += playerStat.jumpHeight;
            jumpCount++;
            isJumping = true;
        }
    }

    void ApplyGravity()
    {
        playerVelocity.y += playerStat.gravityValue * Time.deltaTime;

        // anim.SetBool("IsJumping", false);
    }


    void sprint()
    {
        if (Input.GetButtonDown("Sprint") && playerStat.currentStamina > 0)// Add stamina check here
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
            updatePlayerUI();
        }
        else
        {
            RegenerateStamina(playerStat.staminaRegenerationRate * Time.deltaTime);
            updatePlayerUI();
        }

    }

    public void ConsumeStamina(float amount)
    {
        playerStat.currentStamina = Mathf.Clamp(playerStat.currentStamina - amount, 0f, playerStat.stamina);
        if (playerStat.currentStamina <= 0f)
        {
            isSprinting = false;
        }
    }

    public void RegenerateStamina(float amount)
    {
        playerStat.currentStamina = Mathf.Clamp(playerStat.currentStamina + amount, 0f, playerStat.stamina);
    }

    public void IncreaseMaxHP(int amount)
    {
        playerStat.HPMax += amount;
        playerStat.HP = playerStat.HPMax;  // Optional: This line refills the player's health after increasing the max HP.
    }

    public void IncreaseMaxStamina(int amount)
    {
        playerStat.stamina += amount;
        playerStat.currentStamina = playerStat.stamina;  // Optional: This line refills the player's stamina after increasing the max stamina.
    }
}

