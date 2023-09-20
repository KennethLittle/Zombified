using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class buttonFunctions : MonoBehaviour
{

    public buttonFunctions loadGameButton;
    public Slider musicSlider, ambiSlider, sfxSlider, mouseSensitivity;
    public int saveSlot = 0;

    public AudioManager audioManager;
    public cameraControls cameraController;
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
        int previousSceneIndex = SceneManager.GetActiveScene().buildIndex - 1;
        SceneManager.LoadScene(previousSceneIndex);
        GameStateManager.instance.ChangeState(GameStateManager.GameState.Playing);
        UIManager.Instance.pauseMenu.SetActive(false);
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
        SaveManager.Instance.SaveGame();
    }

    public void LoadGame()
    {
        SaveManager.Instance.LoadGame();
    }

    public void NewGame()
    {
        SaveManager.Instance.NewGame();
    }

    public void ToggleMuscic()
    {
        audioManager.ToggleMusic();

    }
    public void ToggleAmbi()
    {
        audioManager.ToggleAmbi();
    }
    public void ToggleSFX()
    {
        audioManager.ToggleSFX();
    }
    public void MusicVolume()
    {
        audioManager.MusicVolume(musicSlider.value);
    }


    public void SFXVolume()
    {
        audioManager.SFXVolume(sfxSlider.value);
    }

    public void AmbiVolume()
    {
        audioManager.AmbiVolume(ambiSlider.value);
    }

    public void MouseSensitivity()
    {

        cameraController.MouseSensitivity((int)mouseSensitivity.value);
    }



}
