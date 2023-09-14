using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    [Header("Player stats")]
    public int HP;
    public int HPMax;
    public float stamina;
    public float currentStamina;
    public float staminaConsumptionRate;
    public float staminaRegenerationRate;
    public int Level;
    public float playerSpeed;
    public float sprintMod;
    public int jumpMax;
    public float jumpHeight;
    public float gravityValue;

    private void Start()
    {
        currentStamina = stamina; // Assuming you want to initialize the currentStamina to stamina's value.
    }
}
