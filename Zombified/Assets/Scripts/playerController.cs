using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Inventoryitem;

public class playerController : MonoBehaviour, IDamage
{
    public InventorySystem playerInventorySystem;
    public InventoryUI playerInventoryUI;

    public GameObject primaryWeaponSlot;
    public GameObject secondaryWeaponSlot;

    [Header("----- Character -----")]
    [SerializeField] CharacterController controller;
    [SerializeField] Animator anim;

    [Header("----- Player stats -----")]
    [SerializeField] int HP;
    [SerializeField] public float stamina;
    [SerializeField] public float currentStamina;
    [SerializeField] public float staminaConsumptionRate;
    [SerializeField] public float staminaRegenerationRate;
    [SerializeField] float playerSpeed;
    [SerializeField] float sprintMod;
    [SerializeField] int jumpMax;
    [SerializeField] float jumpHeight;
    [SerializeField] float gravityValue;
    [SerializeField] float animChangeSpeed;

    [SerializeField] public int defaultHP = 100;
    [SerializeField] public float defaultStamina = 100;

    [Header("----- Player Gun Stats -----")]
    [SerializeField] List<WeaponStats> weaponList = new List<WeaponStats>();
    [SerializeField] float shootRate;
    [SerializeField] int shootDamage;
    [SerializeField] int shootDist;
    [SerializeField] GameObject weaponmod;
    public int Weaponselected;

    [Header("----- Ammo Box -----")]
    public int ammoBoxAmount;

    [Header("----- Player Med Packs -----")]
    [SerializeField] List<medPackStats> medPackList = new List<medPackStats>();
    public int medPackMaxAmount;
    [SerializeField] int healAmount;
    public int medPackAmount;

    [Header("----- Audio -----")]
    // audio<something> is an array of sfx
    // audio<something>Vol is the sfx volume
    [SerializeField] AudioSource audioSFX;
    [SerializeField] AudioClip[] audioFootsteps;
    [SerializeField] [Range(0, 1)] float footstepsRate;
    [SerializeField] [Range(0, 1)] float audioFootstepsVol;
    [SerializeField] AudioClip[] audioJump;
    [SerializeField] [Range(0, 1)] float audioJumpVol;
    [SerializeField] AudioClip[] audioDamage;
    [SerializeField] [Range(0, 1)] float audioDamageVol;
    [SerializeField] AudioClip[] audioLowHealth;
    [SerializeField] [Range(0, 1)] float audioLowHealthVol;
    
    [SerializeField] AudioClip[] audioShoot;
    [SerializeField] [Range(0, 1)] float audioShootVol;
    [SerializeField] AudioClip[] audioShootCasing;
    [SerializeField] [Range(0, 1)] float audioShootCasingVol;
    [SerializeField] AudioClip[] audioGunReload;
    [SerializeField] [Range(0, 1)] float audioGunReloadVol;

    private float originalPlayerSpeed;
    private int HPMax;
    private bool groundedPlayer;
    private Vector3 move;
    private Vector3 playerVelocity;
    private int jumpCount;
    private bool isSprinting;
    private bool isShooting;
    private bool footstepsIsPlaying;
    private float audioLHVolOrig;
    private bool lowHealthIsPlaying;
    private float lastJumpTime = 0f;
    private float jumpCooldown = 3f;
    private float animVelocity = 0.0f;
    private int velocityHash;
    private float deceleration = 0.5f;
    private float acceleration = 0.6f;

    private void Start()
    {
        
        anim = GetComponent<Animator>();
        velocityHash = Animator.StringToHash("animVelocity");
        originalPlayerSpeed = playerSpeed;
        HPMax = HP;
        currentStamina = stamina;
        defaultHP = HP;
        defaultStamina = stamina;
        audioLHVolOrig = audioLowHealthVol;
        gameManager.instance.medPackMax.text = medPackMaxAmount.ToString("F0"); 
        spawnPlayer();
        if(!playerInventorySystem)
        {
            playerInventorySystem = FindObjectOfType<InventorySystem>();
        }
        if(!playerInventoryUI)
        {
            playerInventoryUI = FindObjectOfType<InventoryUI>();
        }
    }

    void Update()
    {
        movement();
        sprint();
        lowHealthSFX();
        weaponselect();
        useMedPack();
        reloadAmmo();

        if (weaponList.Count > 0 && Input.GetButton("Shoot") && !isShooting)
        {
            StartCoroutine(shoot());
        }
        if (!isShooting)
        {
            anim.SetBool("IsShooting", false);
        }

    }

    public void ResetToDefaults()
    {
        HP = defaultHP;
        stamina = defaultStamina;
    }

    public void SetInitialStats(int level, int hp, int stamina)
    {
        gameManager.instance.levelUpSystem.playerLevel = level;
        defaultHP = hp;
        defaultStamina = stamina;
    }

    public void takeDamage(int amount)
    {
        // Plays damaged audio sfx - Plays a random damaged sfx from the range audioDamage at a volume defined by audioDamageVol 
        HP -= amount;
        StartCoroutine(gameManager.instance.playerFlashDamage());
        updatePlayerUI();
        
        if (HP <= 0)
        {
            anim.SetTrigger("IsDead");
            gameManager.instance.youLose();
            gameManager.instance.levelUpSystem.MarkRunEnd();
        }

        audioSFX.PlayOneShot(audioDamage[Random.Range(0, audioDamage.Length)], audioDamageVol);
    }

    void lowHealthSFX()
    {
        if (!lowHealthIsPlaying && HP <= (HPMax * 0.3))
        {
            StartCoroutine((playLowHealth()));
        }
    }

    IEnumerator playLowHealth()
    {
        lowHealthIsPlaying = true;
        // Plays low health audio sfx - Plays a random footsteps sfx from the range audioLowHealth at a volume defined by audioLowHealthVol
        audioSFX.PlayOneShot(audioLowHealth[Random.Range(0, audioLowHealth.Length)], audioLowHealthVol);
        if (HP <= (HPMax * 0.3) && HP > (HPMax * 0.2))
        {
            audioLowHealthVol = audioLHVolOrig + 0.2f;
            yield return new WaitForSeconds(2.0f);
        }
        else if (HP <= (HPMax * 0.2) && HP > (HPMax * 0.1))
        {
            audioLowHealthVol = audioLHVolOrig + 0.4f;
            yield return new WaitForSeconds(1.5f);
        }
        else if (HP <= (HPMax * 0.1) && HPMax > 0)
        {
            audioLowHealthVol = audioLHVolOrig + 0.6f;
            yield return new WaitForSeconds(1.0f);
        }
        lowHealthIsPlaying = false;
    }

    void movement()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer)
        {
            if (!footstepsIsPlaying && move.normalized.magnitude > 0.5f && HP > 0)
            {
                StartCoroutine(playFootsteps());
            }

            if (playerVelocity.y < 0)
            {
                playerVelocity.y = 0f;
                jumpCount = 0;
            }

            // Detect if the player is moving or not and set animation parameters accordingly
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            float effectivePlayerSpeed = isSprinting ? originalPlayerSpeed * sprintMod : originalPlayerSpeed;

            move = (horizontalInput * transform.right) + (verticalInput * transform.forward);

            if (move != Vector3.zero)
            {
                if (isSprinting && currentStamina > 0) // Modified check here
                {
                    anim.SetBool("IsIdle", false);
                    anim.SetBool("IsWalking", false);
                    anim.SetBool("IsRunning", true);
                    controller.Move(move * Time.deltaTime * playerSpeed * effectivePlayerSpeed); // Running speed
                }
                else
                {
                    isSprinting = false; // Add this line to ensure that sprinting is turned off if stamina is depleted
                    anim.SetBool("IsIdle", false);
                    anim.SetBool("IsRunning", false);
                    anim.SetBool("IsWalking", true);
                    controller.Move(move * Time.deltaTime * playerSpeed); // Walking speed
                }
            }
            else
            {
                anim.SetBool("IsWalking", false);
                anim.SetBool("IsRunning", false);
                anim.SetBool("IsIdle", true);
                if (animVelocity < 0.0f)
                {
                    animVelocity = 0.0f;
                }
            }
            playerVelocity.x = move.x * effectivePlayerSpeed; 
            playerVelocity.z = move.z * effectivePlayerSpeed;

            anim.SetFloat(velocityHash, animVelocity);
        }
        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax && Time.time - lastJumpTime > jumpCooldown)
        {
            lastJumpTime = Time.time;
            // Plays jump audio sfx - Plays a random jump sfx from the range audioJump at a volume defined by audioJumpVol
            audioSFX.PlayOneShot(audioJump[Random.Range(0, audioJump.Length)], audioJumpVol);

            
            playerVelocity.y += jumpHeight;
            anim.SetBool("IsJumping", true);
            jumpCount++;
        }

        playerVelocity.x = move.x * playerSpeed; // Maintain the horizontal components
        playerVelocity.z = move.z * playerSpeed;

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        // Reset the IsJumping animation parameter if the player is grounded
        if (groundedPlayer)
        {
            anim.SetBool("IsJumping", false);
        }
    }

    // Play footsteps sfx at a rate defined by footstepsRate
    IEnumerator playFootsteps()
    {
        footstepsIsPlaying = true;
        // Plays footsteps audio sfx - Plays a random footsteps sfx from the range audioFootsteps at a volume defined by audioFootstepsVol
        audioSFX.PlayOneShot(audioFootsteps[Random.Range(0, audioFootsteps.Length)], audioFootstepsVol);
        if (!isSprinting)
            yield return new WaitForSeconds(footstepsRate);
        else
            yield return new WaitForSeconds(footstepsRate / 2);
        footstepsIsPlaying = false;
    }

    void sprint()
    {
        if (Input.GetButtonDown("Sprint") && currentStamina > 0) // Add stamina check here
        {
            isSprinting = true;
        }
        else if (Input.GetButtonUp("Sprint") || currentStamina <= 0) // Add stamina check here
        {
            isSprinting = false;
        }

        if (isSprinting)
        {
            ConsumeStamina(staminaConsumptionRate * Time.deltaTime);
        }
        else
        {
            RegenerateStamina(staminaRegenerationRate * Time.deltaTime);
        }
    }

    public void ConsumeStamina(float amount)
    {
        currentStamina = Mathf.Clamp(currentStamina - amount, 0f, stamina);

        if (currentStamina <= 0f)
        {
            isSprinting = false;
        }

        updatePlayerUI();
    }

    public void RegenerateStamina(float amount)
    {
        currentStamina = Mathf.Clamp(currentStamina + amount, 0f, stamina);

        updatePlayerUI();
    }

    IEnumerator shoot()
    {
        if (weaponList[Weaponselected].ammoCur > 0)
        {
            isShooting = true;
            anim.SetBool("IsShooting", true);
            weaponList[Weaponselected].ammoCur--;
            updatePlayerUI();

            // Plays gunshot audio sfx - Plays a random gunshot sfx from the range audioShoot at a volume defined by audioShootVol
            audioSFX.PlayOneShot(audioShoot[Random.Range(0, audioShoot.Length)], audioShootVol);
            // Plays gunshot casing audio sfx - Plays a random gunshot casing sfx from the range audioShootCasing at a volume defined by audioShootCasingVol
            audioSFX.PlayOneShot(audioShootCasing[Random.Range(0, audioShootCasing.Length)], audioShootCasingVol);

            // shoot code
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDist))
            {
                IDamage damageable = hit.collider.GetComponent<IDamage>();

                if (damageable != null)
                {
                    damageable.takeDamage(shootDamage);
                }
            }

            yield return new WaitForSeconds(shootRate);
            isShooting = false; 

        }
    }

    void useMedPack()
    {
        if(Input.GetKeyDown(KeyCode.Q) && medPackAmount > 0 && HP < HPMax)
        {
            medPackAmount--;
            gameManager.instance.medPackCur.text = medPackAmount.ToString("F0");
            HP += healAmount;
            if (HP > HPMax)
            {
                HP = HPMax;
            }
            updatePlayerUI();
        }
    }

    void reloadAmmo()
    {
        if (Input.GetKeyDown(KeyCode.R) && weaponList[Weaponselected].ammoCur < weaponList[Weaponselected].ammoMax && ammoBoxAmount > 0)
        {
            int difference = weaponList[Weaponselected].ammoMax - weaponList[Weaponselected].ammoCur;
            anim.SetBool("IsReloading", true);

            if (ammoBoxAmount >= difference)
            {
                weaponList[Weaponselected].ammoCur = weaponList[Weaponselected].ammoMax;
                ammoBoxAmount -= difference;
            }
            else
            {
                weaponList[Weaponselected].ammoCur += ammoBoxAmount;
                ammoBoxAmount = 0; // Since all the ammo in the box was used, set it to 0
            }

            gameManager.instance.ammoBoxAmount.text = ammoBoxAmount.ToString("F0");
            anim.SetBool("IsReloading", false);
        }
    }

    public void spawnPlayer()
    {
        controller.enabled = false;
        transform.position = gameManager.instance.playerSpawnPos.transform.position;
        controller.enabled = true;
        HP = HPMax;
        audioLowHealthVol = audioLHVolOrig;
        updatePlayerUI();
    }

    public void IncreaseMaxHP(int amount)
    {
        if (!gameManager.instance.levelUpSystem.isInRun)
        {
             
            HPMax += amount;
            HP += amount;
            updatePlayerUI();
        }
    }

    public void IncreaseMaxStamina(int amount)
    {
        if (!gameManager.instance.levelUpSystem.isInRun)
        {
            
            stamina += amount;
            currentStamina += amount;
            updatePlayerUI();
        }
    }

    public void updatePlayerUI()
    {
        gameManager.instance.playerHPBar.fillAmount = (float)HP / HPMax;
        gameManager.instance.staminaBar.fillAmount = currentStamina / stamina;

        if(weaponList.Count > 0)
        {
            gameManager.instance.ammoCur.text = weaponList[Weaponselected].ammoCur.ToString("F0");
            gameManager.instance.ammoMax.text = weaponList[Weaponselected].ammoMax.ToString("F0");
            gameManager.instance.weaponIcon.sprite = weaponList[Weaponselected].icon;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        Inventoryitem item = other.GetComponent<Inventoryitem>();
        if(item)
        {
            if(playerInventorySystem.AddItem(item))
            {
                Destroy(item.gameObject);
            }
        }
    }

    public void weaponpickup(WeaponStats weaponStat)
    {
        weaponList.Add(weaponStat);
        anim.SetBool("weaponEquipped", true);

        shootDamage = weaponStat.shootDamage;
        shootRate = weaponStat.shootRate;
        shootDist = (int)weaponStat.shootDist;

        audioShoot = weaponStat.audioShoot;
        audioShootVol = weaponStat.audioShootVol;
        audioShootCasing = weaponStat.audioShootCasing;
        audioShootCasingVol = weaponStat.audioShootCasingVol;
        audioGunReload = weaponStat.audioGunReload;
        audioGunReloadVol = weaponStat.audioGunReloadVol;

        weaponmod.GetComponent<MeshFilter>().sharedMesh = weaponStat.model.GetComponent<MeshFilter>().sharedMesh;
        weaponmod.GetComponent<MeshRenderer>().sharedMaterial = weaponStat.model.GetComponent<MeshRenderer>().sharedMaterial;

        updatePlayerUI();
    }
    public void EquipWeapon(Inventoryitem weapon)
    {
        if (weapon.itemType == ItemType.PrimaryWeapon && !primaryWeaponSlot.transform)
        {
            GameObject newWeapon = Instantiate(weapon.gameObject, primaryWeaponSlot.transform);
        }
        else if (weapon.itemType == ItemType.SecondaryWeapon && !secondaryWeaponSlot.transform)
        {
            GameObject NewWeapon = Instantiate(weapon.gameObject, secondaryWeaponSlot.transform);
        }
    }

    public void medPackPickup(medPackStats medPackStat)
    {
        if (medPackAmount < medPackMaxAmount)
        {
            medPackList.Add(medPackStat);
            medPackAmount++;

            gameManager.instance.medPackCur.text = medPackAmount.ToString("F0");
        }

    }

    public void ammoBoxPickup(ammoBoxStats ammoBoxStat)
    {
        ammoBoxAmount += ammoBoxStat.ammoAmount;
        gameManager.instance.ammoBoxAmount.text = ammoBoxAmount.ToString("F0");
    }

    void weaponselect()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && Weaponselected < weaponList.Count - 1)
        {
            Weaponselected++;
            changeweapon();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && Weaponselected > 0)
        {
            Weaponselected--;
            changeweapon();
        }
    }

    public void UseItem(Inventoryitem item)
    {
        if(item.itemType == ItemType.General)
        {
            if(item.name == "MedPack")
            {
                if (HP > HPMax)
                {
                    useMedPack();
                    playerInventorySystem.RemoveItem(item);
                }
            }
            else if(item.name == "Ammo Box")
            {
                reloadAmmo();
            }
        }
    }
    void changeweapon()
    {
        shootDamage = weaponList[Weaponselected].shootDamage;
        shootDist = (int)weaponList[Weaponselected].shootDist;
        shootRate = weaponList[Weaponselected].shootRate;

        audioShoot = weaponList[Weaponselected].audioShoot;
        audioShootVol = weaponList[Weaponselected].audioShootVol;
        audioShootCasing = weaponList[Weaponselected].audioShootCasing;
        audioShootCasingVol = weaponList[Weaponselected].audioShootCasingVol;
        audioGunReload = weaponList[Weaponselected].audioGunReload;
        audioGunReloadVol = weaponList[Weaponselected].audioGunReloadVol;
        
        weaponmod.GetComponent<MeshFilter>().sharedMesh = weaponList[Weaponselected].model.GetComponent<MeshFilter>().sharedMesh;
        weaponmod.GetComponent<MeshRenderer>().sharedMaterial = weaponList[Weaponselected].model.GetComponent<MeshRenderer>().sharedMaterial;

        updatePlayerUI();
    }

}
