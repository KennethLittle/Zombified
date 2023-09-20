using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    [Header("----- Components -----")]
    public static PlayerStat Instance;

    [Header("Player stats")]
    public int HP;
    public int HPMax;
    public float stamina;
    public float currentStamina;
    public float staminaConsumptionRate;
    public float staminaRegenerationRate;
    public int Level;

    [Header("----- Player Movement -----")]
    public float playerSpeed;
    public float sprintMod;
    public int jumpMax;
    public float jumpHeight;
    public float gravityValue;
   

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
