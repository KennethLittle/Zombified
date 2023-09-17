using JetBrains.Annotations;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI Elements")]
    public GameObject activeMenu;
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
    public Canvas mainUICanvas;

    public GameObject pauseMenu;
    // Add other UI references as required

    [Header("Manager References")]
    public gameManager gameManagerInstance;
    public GameStateManager gameStateManager;
    public QuestUIManager questUIManager;

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

    private void Update()
    {
        TogglePauseMenu();

        if (Input.GetKeyDown(KeyCode.Q)) // Let's say Q is the key to toggle the quest tracker.
        {
            ToggleQuestTracker();
        }
    }

    public void ToggleUI(bool isActive)
    {
        mainUICanvas.gameObject.SetActive(isActive);
        // Do this for other canvases if you have more than one.
    }

    public void TogglePauseMenu()
    {
       if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameStateManager.instance.ChangeState(GameStateManager.GameState.Paused);
            pauseMenu.SetActive(true);
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

    public void ToggleQuestTracker()
    {
        
    }

    // Add other UI-related methods as required
}
