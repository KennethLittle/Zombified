using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class playerController : MonoBehaviour, IDamage
{
    public PlayerStat playerStat;
    public Transform weaponSlot;
    GameObject inventory;
    GameObject craftSystem;
    GameObject characterSystem;


    public PlayerEquipment playerInventoryUI;
    public InventoryItem currentEquippedItem;

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

    //private bool playingTakeDamageSFX = false;

    private void Start()
    {

        originalPlayerSpeed = playerStat.playerSpeed;
        playerStat.HP = playerStat.HPMax;
        playerStat.currentStamina = playerStat.stamina;

        //this is for the changing the rate and volume of footsteps
        foreach (var sound in AudioManager.instance.PlayerSounds)
        {
            if (sound.name == "Footsteps")
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
        if (Input.GetButtonDown("Shooting") && !isShooting)
        {
            StartCoroutine(Shooting());
            Debug.Log("Shooting");
        }

    }

    public void EquipItem(InventoryItem item)
    {
        currentEquippedItem = item;
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

    int GetDamageFromItemStats()
    {
        foreach (ItemStats stat in currentEquippedItem.itemStats)
        {
            if (stat.attributeName == "Damage")  // Compare attributeName to "Damage" string
            {
                return stat.attributeValue;  // Return the attributeValue for damage
            }
        }
        return 0;  // Default to no damage if not found
    }

    IEnumerator Shooting()
    { 
        while (PlayerEquipment.Instance.equippedWeapon != null)
        {
            isShooting = true;

            float fireRate = currentEquippedItem.fireRate;
            float shootDist = currentEquippedItem.range;
            int damage = GetDamageFromItemStats();

            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDist))
            {
                IDamage damageable = hit.collider.GetComponent<IDamage>();
                if (damageable != null)
                {
                    damageable.takeDamage(damage);
                }

            }
            float fireInterval = 1.0f / PlayerEquipment.Instance.equippedWeapon.weaponDetails.fireRate;
            yield return new WaitForSeconds(fireInterval);
            isShooting = false;
        }
    }
   

    public void takeDamage(int amount)
    {
        // Plays damaged audio sfx - Plays a random damaged sfx from the range audioDamage at a volume defined by audioDamageVol 
        playerStat.HP -= amount;
        updatePlayerUI();
        StartCoroutine(UIManager.Instance.PlayerFlashDamage());
        if (playerStat.HP <= 0)
        {
            anim.SetTrigger("IsDead");
        }
        //else
        //{
        //    if (!playingTakeDamageSFX)
        //    {
        //        AudioManager.instance.PlaySound("Take Damage", AudioManager.instance.PlayerSounds);
        //        playingTakeDamageSFX = true;
        //    }
        //    else if (playingTakeDamageSFX)
        //    {
        //        Invoke("takeDamageSFXFinished", AudioManager.instance.PlayerSounds[5].clip.length);
        //    }
        //}
    }

    public void updatePlayerUI()
    {
        UIManager.Instance.playerHPBar.fillAmount = (float)PlayerStat.Instance.HP / PlayerStat.Instance.HPMax;
        UIManager.Instance.staminaBar.fillAmount = (float)PlayerStat.Instance.currentStamina / PlayerStat.Instance.stamina;
    }

    //void takeDamageSFXFinished()
    //{
    //    playingTakeDamageSFX = false;
    //    Debug.Log("Audio Finished");
    //}
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

    }

    public void IncreaseMaxHP(int amount)
    {
        if (!gameManager.instance.isInRun)
        {

            playerStat.HPMax += amount;
            playerStat.HP += amount;

        }
    }

    public void IncreaseMaxStamina(int amount)
    {
        if (!gameManager.instance.isInRun)
        {

            playerStat.stamina += amount;
            playerStat.currentStamina += amount;

        }
    }

}
