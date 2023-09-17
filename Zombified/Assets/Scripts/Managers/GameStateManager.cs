using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager instance;

    public enum GameState { Playing, Paused, Ended, Escape }

    public GameState currentState;

    [Header("Manager References")]
    public UIManager uiManager;
    public gameManager gameManagerInstance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.LogError("Multiple instances of GameStateManager found. Destroying one.");
            Destroy(gameObject);
        }
    }

    public void ChangeState(GameState newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case GameState.Playing:
                Time.timeScale = 1;
                Cursor.lockState = CursorLockMode.Locked;
                break;
            case GameState.Paused:
                Time.timeScale = 0;
                
                Cursor.lockState = CursorLockMode.Confined;
                break;
        }
    }

    public void HandleDefeat()
    {
        ChangeState(GameStateManager.GameState.Ended);
    }

    public void HandleEscape()
    {
        ChangeState(GameStateManager.GameState.Escape);
    }

    // Other game state related methods as required...
}
