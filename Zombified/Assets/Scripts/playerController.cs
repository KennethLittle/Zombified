using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour, IDamage
{
    [SerializeField] CharacterController controller;

    [SerializeField] int HP;
    [SerializeField] float playerSpeed;
    [SerializeField] float sprintMod;
    [SerializeField] int jumpMax;
    [SerializeField] float jumpHeight;
    [SerializeField] float gravityValue;

    [SerializeField] float shootRate;
    [SerializeField] int shootDamage;
    [SerializeField] int shootDist;


    private bool groundedPlayer;
    private Vector3 move;
    private Vector3 playerVelocity;
    private int jumpCount;
    private bool isSprinting;
    private bool isShooting;

    private void Start()
    {

    }

    void Update()
    {
        movement();
        sprint();
        if (Input.GetButton("Shoot") && !isShooting)
            StartCoroutine(shoot());
    }

    public void takeDamage(int amount)
    {
        HP -= amount;
    }

    void movement()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
            jumpCount = 0;
        }

        move = (Input.GetAxis("Horizontal") * transform.right) +
               (Input.GetAxis("Vertical") * transform.forward);
        controller.Move(move * Time.deltaTime * playerSpeed);


        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
            playerVelocity.y = jumpHeight;
            jumpCount++;
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
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
    }

    IEnumerator shoot()
    {
        isShooting = true;

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
