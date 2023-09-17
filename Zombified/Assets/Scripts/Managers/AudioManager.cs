using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Sound[] mainMenuTracks, homeBaseTracks, alphaStainTracks, PlayerSounds, itemSFXSounds, levelSFXSounds, enemySFXSounds, NPCSFXSounds, UI_MenuSFXSounds;
    public AudioSource musicSource, SFXSource;

    private List<int> playedSongs;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "MainMenu")
        {

            PlayMusic("MainMenu", mainMenuTracks);
        }
        else PlayRandomSong();

    }

    public void PlayMusic(string name, Sound[] soundArray)
    {
        Sound s = Array.Find(soundArray, x => x.name == name);
        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }
    public void PlaySound(string name, Sound[] soundArray)
    {
        Sound s = Array.Find(soundArray, x => x.name == name);
        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            SFXSource.clip = s.clip;
            SFXSource.Play();
        }
    }
 
    private void PlayRandomSong()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "HomeBase")
        {
            int randomSong = UnityEngine.Random.Range(0, homeBaseTracks.Length);
            AudioClip songPlaying = homeBaseTracks[randomSong].clip;
            musicSource.clip = songPlaying;
            musicSource.Play();

            Invoke("PlayRandomSong", songPlaying.length);
        }
        else if (currentScene.name == "Alpha stain")
        {
            int randomSong = UnityEngine.Random.Range(0,alphaStainTracks.Length);
            AudioClip songPlaying = alphaStainTracks[randomSong].clip;
            musicSource.clip = songPlaying;
            musicSource.Play();

            Invoke("PlayRandomSong", songPlaying.length);
        }

    }
   

    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
    }

    public void ToggleSFX()
    {
        SFXSource.mute = !SFXSource.mute;
    }

    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void SFXVolume(float volume)
    {
        SFXSource.volume = volume;
    }
}
