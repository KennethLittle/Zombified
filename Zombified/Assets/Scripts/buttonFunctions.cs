using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class buttonFunctions : MonoBehaviour
{

    public buttonFunctions loadGameButton;
    public Slider musicSlider, sfxSlider;
    private SaveUIManager saveUIManager;

    public void resume()
    {
        GameStateManager.instance.ChangeState(GameStateManager.GameState.Playing);
        UIManager.Instance.pauseMenu.SetActive(false);
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

    public void ReturntoHomeBase()
    {
        PlayerData data = new PlayerData(PlayerManager.instance);
        PlayerManager.TempPlayerData = data;
        int previousSceneIndex = SceneManager.GetActiveScene().buildIndex - 1;
        SceneManager.LoadScene(previousSceneIndex);
        GameStateManager.instance.ChangeState(GameStateManager.GameState.Playing);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
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
        gameManager.instance.SaveGameState();
    }

    public void LoadGame()
    {
        // After loading the game state, activate the SaveUIManager's menu
       gameManager.instance.LoadGameState();
        UIManager.Instance.pauseMenu.SetActive(false);
        GameStateManager.instance.ChangeState(GameStateManager.GameState.Playing);

    }

    public void NewGame()
    {

        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        gameManager.instance.StartNewGame();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void ToggleMuscic()
    {
        AudioManager.instance.ToggleMusic();
    }
    public void ToggleSFX()
    {
        AudioManager.instance.ToggleSFX();
    }

    public void MusicVolume()
    {
        AudioManager.instance.MusicVolume(musicSlider.value);
    }

    public void SFXVolume()
    {
        AudioManager.instance.SFXVolume(sfxSlider.value);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


}
