using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour, IDamage
{
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

    [Header("----- Player Gun Stats -----")]
    [SerializeField] List<WeaponStats> weaponList = new List<WeaponStats>();
    [SerializeField] float shootRate;
    [SerializeField] int shootDamage;
    [SerializeField] int shootDist;
    [SerializeField] GameObject weaponmod;
    public int Weaponselected;


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


    private void Start()
    {
        HPMax = HP;
        currentStamina = stamina;
        audioLHVolOrig = audioLowHealthVol;
        spawnPlayer();
    }

    void Update()
    {
        movement();
        sprint();
        lowHealthSFX();
        weaponselect();

        if (weaponList.Count > 0 && Input.GetButton("Shoot") && !isShooting)
        {
            StartCoroutine(shoot());
        }
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
            move = (horizontalInput * transform.right) + (verticalInput * transform.forward);
            if (move != Vector3.zero)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    anim.SetBool("IsRunning", true);
                    anim.SetBool("IsWalking", false);
                    anim.SetBool("IsIdle", false);
                    controller.Move(move * Time.deltaTime * playerSpeed * 1.5f); // Running speed
                }
                else
                {
                    anim.SetBool("IsWalking", true);
                    anim.SetBool("IsRunning", false);
                    anim.SetBool("IsIdle", false);
                    controller.Move(move * Time.deltaTime * playerSpeed); // Walking speed
                }
            }
            else
            {
                anim.SetBool("IsIdle", true);
                anim.SetBool("IsWalking", false);
                anim.SetBool("IsRunning", false);
            }
        }

        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax && Time.time - lastJumpTime > jumpCooldown)
        {
            lastJumpTime = Time.time;
            // Plays jump audio sfx - Plays a random jump sfx from the range audioJump at a volume defined by audioJumpVol
            audioSFX.PlayOneShot(audioJump[Random.Range(0, audioJump.Length)], audioJumpVol);

            playerVelocity.y = jumpHeight;
            jumpCount++;
            anim.SetBool("IsJumping", true);
        }

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
        if (Input.GetButtonDown("Sprint"))
        {
            isSprinting = true;
            
            playerSpeed *= sprintMod;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            isSprinting = false;
            
            playerSpeed /= sprintMod;
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
        isShooting = true;
        anim.SetTrigger("IsRecoiling");
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
            //gameManager.instance.ammoCur.text = weaponList[Weaponselected].ammoCur.ToString("F0");
            //gameManager.instance.ammoMax.text = weaponList[Weaponselected].ammoMax.ToString("F0");
        }
    }

    public void weaponpickup(WeaponStats weaponStat)
    {
        weaponList.Add(weaponStat);

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
