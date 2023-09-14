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

    public InventorySystem playerInventorySystem;
    public InventoryUI playerInventoryUI;

    [Header("----- Character -----")]
    [SerializeField] CharacterController controller;
    [SerializeField] Animator anim;


    [Header("----- Player Gun Stats -----")]
    [SerializeField] public List<GameObject> equippedWeapons = new List<GameObject>(); // List of instantiated weapon game objects
    public int currentWeaponIndex = 0;

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

    public Quest quest;
    public QuestGoal currQuest;

    private float originalPlayerSpeed;
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
        originalPlayerSpeed = playerStat.playerSpeed;
        playerStat.HPMax = playerStat.HP;
        playerStat.currentStamina = playerStat.stamina;
        audioLHVolOrig = audioLowHealthVol;
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
        useMedPack();
        reloadAmmo();

        if (weaponSlot.transform.childCount > 0 && Input.GetButton("Shoot") && !isShooting)
        {
            StartCoroutine(shoot());
            anim.SetBool("IsShooting", false);
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

        audioSFX.PlayOneShot(audioDamage[Random.Range(0, audioDamage.Length)], audioDamageVol);
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
        audioSFX.PlayOneShot(audioLowHealth[Random.Range(0, audioLowHealth.Length)], audioLowHealthVol);
        if (playerStat.HP <= (playerStat.HPMax * 0.3) && playerStat.HP > (playerStat.HPMax * 0.2))
        {
            audioLowHealthVol = audioLHVolOrig + 0.2f;
            yield return new WaitForSeconds(2.0f);
        }
        else if (playerStat.HP <= (playerStat.HPMax * 0.2) && playerStat.HP > (playerStat.HPMax * 0.1))
        {
            audioLowHealthVol = audioLHVolOrig + 0.4f;
            yield return new WaitForSeconds(1.5f);
        }
        else if (playerStat.HP <= (playerStat.HPMax * 0.1) && playerStat.HPMax > 0)
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
            if (!footstepsIsPlaying && move.normalized.magnitude > 0.5f && playerStat.HP > 0)
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
            float effectivePlayerSpeed = isSprinting ? originalPlayerSpeed * playerStat.sprintMod : originalPlayerSpeed;

            move = (horizontalInput * transform.right) + (verticalInput * transform.forward);

            if (move != Vector3.zero)
            {
                if (isSprinting && playerStat.currentStamina > 0) // Modified check here
                {
                    anim.SetBool("IsIdle", false);
                    anim.SetBool("IsWalking", false);
                    anim.SetBool("IsRunning", true);
                    controller.Move(move * Time.deltaTime * playerStat.playerSpeed * effectivePlayerSpeed); // Running speed
                }
                else
                {
                    isSprinting = false; // Add this line to ensure that sprinting is turned off if stamina is depleted
                    anim.SetBool("IsIdle", false);
                    anim.SetBool("IsRunning", false);
                    anim.SetBool("IsWalking", true);
                    controller.Move(move * Time.deltaTime * playerStat.playerSpeed); // Walking speed
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
        if (Input.GetButtonDown("Jump") && jumpCount < playerStat.jumpMax && Time.time - lastJumpTime > jumpCooldown)
        {
            lastJumpTime = Time.time;
            // Plays jump audio sfx - Plays a random jump sfx from the range audioJump at a volume defined by audioJumpVol
            audioSFX.PlayOneShot(audioJump[Random.Range(0, audioJump.Length)], audioJumpVol);

            
            playerVelocity.y += playerStat.jumpHeight;
            anim.SetBool("IsJumping", true);
            jumpCount++;
        }

        playerVelocity.x = move.x * playerStat.playerSpeed; // Maintain the horizontal components
        playerVelocity.z = move.z * playerStat.playerSpeed;

        playerVelocity.y += playerStat.gravityValue * Time.deltaTime;
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
            anim.SetBool("IsShooting", true);

            weapon.ammoCur--;
            updatePlayerUI();

            // Plays gunshot audio sfx
            audioSFX.PlayOneShot(weapon.audioShoot[Random.Range(0, weapon.audioShoot.Length)], weapon.audioShootVol);

            // Plays gunshot casing audio sfx
            audioSFX.PlayOneShot(weapon.audioShootCasing[Random.Range(0, weapon.audioShootCasing.Length)], weapon.audioShootCasingVol);

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


    void useMedPack()
    {
        BaseItemStats medPack = InventorySystem.Instance.items.Find(item => item is medPackStats);

        if (Input.GetKeyDown(KeyCode.Q) && medPack != null &&  playerStat.HP < playerStat.HPMax)
        {
            // Get the specific MedPack properties.
            medPackStats specificMedPack = medPack as medPackStats;
            int healAmount = specificMedPack.healAmount;

            playerStat.HP += healAmount;

            if (playerStat.HP > playerStat.HPMax)
            {
                playerStat.HP = playerStat.HPMax;
            }

            // Update UI and remove the med pack from inventory after use.
            updatePlayerUI();
            InventorySystem.Instance.RemoveItem(medPack);
        }
    }

    void reloadAmmo()
    {
        WeaponStats currentWeapon = GetCurrentWeaponStats();

        if (currentWeapon == null)
            return; // No weapon currently equipped or selected

        if (Input.GetKeyDown(KeyCode.R) && currentWeapon.ammoCur < currentWeapon.ammoMax)
        {
            int difference = currentWeapon.ammoMax - currentWeapon.ammoCur;
            anim.SetBool("IsReloading", true);

            while (difference > 0 && InventorySystem.Instance.items.Any(item => item is ammoBoxStats))
            {
                ammoBoxStats ammoBox = (ammoBoxStats)InventorySystem.Instance.items.First(item => item is ammoBoxStats);
                int ammoToTake = Mathf.Min(difference, ammoBox.ammoAmount);

                currentWeapon.ammoCur += ammoToTake;
                ammoBox.ammoAmount -= ammoToTake;

                difference -= ammoToTake;

                if (ammoBox.ammoAmount <= 0) // Remove ammo box if empty
                {
                    InventorySystem.Instance.RemoveItem(ammoBox);
                }
            }

            anim.SetBool("IsReloading", false);
            updatePlayerUI(); // To reflect the changes in the UI
        }
    }

    public void spawnPlayer()
    {
        controller.enabled = false;
        transform.position = PlayerManager.instance.playerSpawnPos.transform.position;
        controller.enabled = true;
        playerStat.HP = playerStat.HPMax;
        audioLowHealthVol = audioLHVolOrig;
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

        int medPackCount = InventorySystem.Instance.items.Count(item => item is medPackStats);
        UIManager.Instance.medPackCur.text = medPackCount.ToString("F0");

        int ammoBoxCount = InventorySystem.Instance.items.Count(item => item is ammoBoxStats);
        UIManager.Instance.ammoBoxAmount.text = ammoBoxCount.ToString("F0");
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
