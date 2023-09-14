using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{

    public buttonFunctions loadGameButton;
    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        if (System.IO.File.Exists(Application.persistentDataPath + "/save.json"))
        {
            loadGameButton.gameObject.SetActive(true);
        }
        else
        {
            loadGameButton.gameObject.SetActive(false);
        }
    }
    public void resume()
    {
        GameStateManager.instance.ChangeState(GameStateManager.GameState.Playing);
    }

    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void quit()
    {
        Application.Quit();
    }

    public void play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); //Kenneth Little
    }

    public void playLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); //Kenneth Littl
    }

    public void quittoMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        
    }

    public void quitToPlayerMenu()
    {
        int previousSceneIndex = SceneManager.GetActiveScene().buildIndex - 1;
        SceneManager.LoadScene(previousSceneIndex);
        GameStateManager.instance.ChangeState(GameStateManager.GameState.Playing);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void next()
    {
        gameManager.instance.levelUpSystem.RewardXPUponDeath();
        gameManager.instance.MarkRunEnd();
        SceneManager.LoadScene("DeathScene");
    }

    public void backtoPlayerMenu()
    {
        int  previousSceneIndex = SceneManager.GetActiveScene().buildIndex - 3;
        SceneManager.LoadScene(previousSceneIndex);
    }

    public void backtoPlayerMenu2()
    {
        int previousSceneIndex = SceneManager.GetActiveScene().buildIndex - 2;
        SceneManager.LoadScene(previousSceneIndex);
    }

    public void SaveGame()
    {
        gameManager.instance.SaveGame();
    }

    public void LoadGame()
    {
        gameManager.instance.LoadGame();
    }

    public void NewGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == SceneManager.GetActiveScene().buildIndex)
        {
            gameManager.instance.StartNewGame();

            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
