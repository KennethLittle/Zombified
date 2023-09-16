using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI Elements")]
    public GameObject activeMenu;
    public GameObject pauseMenu;
    public GameObject loseMenu;
    public GameObject escapeMenu;
    public GameObject controlMenu;
    public TextMeshProUGUI enemiesRemainingText;
    public TextMeshProUGUI waveNumberText;
    public Image playerHPBar;
    public Image staminaBar;
    public Image dialogueBox;
    [SerializeField] GameObject playerDamageFlash;
    public TextMeshProUGUI ammoCur;
    public TextMeshProUGUI ammoMax;
    public TextMeshProUGUI medPackCur;
    public TextMeshProUGUI medPackMax;
    public TextMeshProUGUI ammoBoxAmount;
    public Image weaponIcon;
    // Add other UI references as required

    [Header("Manager References")]
    public gameManager gameManagerInstance;
    public GameStateManager gameStateManager;

    private void Awake()
    {
        // Ensure that there's only one SaveManager instance
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Rest of your initialization
    }

    public void TogglePauseMenu()
    {
        if (gameStateManager.currentState == GameStateManager.GameState.Playing)
        {
            gameStateManager.ChangeState(GameStateManager.GameState.Paused);
        }
        else if (gameStateManager.currentState == GameStateManager.GameState.Paused)
        {
            gameStateManager.ChangeState(GameStateManager.GameState.Playing);
        }
    }

    public void ToggleControlMenu()
    {
        controlMenu.SetActive(!controlMenu.activeSelf);
    }

    public void UpdateEnemiesRemainingText(int count)
    {
        enemiesRemainingText.text = count.ToString();
    }

    public void SetActiveMenu(GameObject menu)
    {
        if (activeMenu != null)
        {
            activeMenu.SetActive(false);
        }

        activeMenu = menu;

        if (activeMenu != null)
        {
            activeMenu.SetActive(true);
        }
    }

    public void UpdatePlayerUI(int currentHP, int maxHP, float currentStamina, float maxStamina)
    {
        playerHPBar.fillAmount = (float)currentHP / maxHP;
        staminaBar.fillAmount = currentStamina / maxStamina;
    }

    public IEnumerator PlayerFlashDamage()
    {
        playerDamageFlash.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        playerDamageFlash.SetActive(false);
    }

    public void showDialogueBox()
    {
        dialogueBox.gameObject.SetActive(true);
    }

    public void hideDialogueBox()
    {
        dialogueBox.gameObject.SetActive(false);

    }

    // Add other UI-related methods as required
}
