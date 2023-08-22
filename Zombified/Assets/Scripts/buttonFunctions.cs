using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour 
{
    public void resume()
    {
        gameManager.instance.stateUnpaused();
    }

    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gameManager.instance.stateUnpaused();
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); //Kenneth Little
    }

    public void quittoMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void quitToPlayerMenu()
    {
        SceneManager.LoadScene("Player and Level Select Screen");
    }

    public void playerRespawn()
    {
        gameManager.instance.playerScript.spawnPlayer();
        gameManager.instance.stateUnpaused();
    }

    public void escape()
    {
        gameManager.instance.levelUpSystem.MarkRunEnd();
        gameManager.instance.levelUpSystem.MarkAsEscaped();
        gameManager.instance.levelUpSystem.RewardXPUponEscape();
        SceneManager.LoadScene("EscapeScene");
    }

    public void next()
    {
        gameManager.instance.levelUpSystem.RewardXPUponDeath();
        gameManager.instance.levelUpSystem.MarkRunEnd();
        SceneManager.LoadScene("DeathScene");
    }

    public void resumeRun()
    {
        gameManager.instance.stateUnpaused();
        WaveManager.instance.ResumeWave();
    }

}
