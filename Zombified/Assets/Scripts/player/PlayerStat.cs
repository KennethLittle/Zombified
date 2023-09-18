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

    public static PlayerStat Instance;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }    
        else
        {
            Destroy(gameObject);
        }

        HP = HPMax;
        currentStamina = stamina; // Assuming you want to initialize the currentStamina to stamina's value.
    }
}
