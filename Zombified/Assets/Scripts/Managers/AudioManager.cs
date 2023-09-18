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

    private float fadeDuration = 2.0f;
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
    //public void PlaySound(string name, Sound[] soundArray)
    //{
    //    Sound s = Array.Find(soundArray, x => x.name == name);
    //    if (s == null)
    //    {
    //        Debug.Log("Sound Not Found");
    //    }
    //    else
    //    {
    //        //SFXSource = gameObject.AddComponent<AudioSource>();
    //        SFXSource.clip = s.clip;
    //        SFXSource.Play();
    //    }
    //}

    public void PlaySound(string name, Sound[] soundArray)
    {
        Sound s = Array.Find(soundArray, x => x.name == name);
        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            AudioSource newSFXSource = gameObject.AddComponent<AudioSource>();
            newSFXSource.clip = s.clip;
            newSFXSource.Play();
            Destroy(newSFXSource, s.clip.length); // destroy the AudioSource once the clip has finished playing
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
            StartCoroutine(FadeIn(songPlaying));
        }
        else if (currentScene.name == "Alpha stain")
        {
            int randomSong = UnityEngine.Random.Range(0,alphaStainTracks.Length);
            AudioClip songPlaying = alphaStainTracks[randomSong].clip;
            musicSource.clip = songPlaying;
            musicSource.Play();
            StartCoroutine(FadeIn(songPlaying));
        }

    }
   
    private IEnumerator FadeIn(AudioClip song)
    {
        float startVolume = 0f;
        float targetVolume = 0.05f;

        musicSource.clip = song;
        musicSource.volume = startVolume;
        musicSource.Play();

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            musicSource.volume = Mathf.Lerp(startVolume, targetVolume, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        musicSource.volume = targetVolume;

        yield return new WaitForSeconds(song.length);

        StartCoroutine(FadeOut() );
    }

    private IEnumerator FadeOut()
    {
        float startVolume = musicSource.volume;
        float targetVolume = 0f;

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            musicSource.volume = Mathf.Lerp(startVolume, targetVolume, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        musicSource.volume = targetVolume;

        PlayRandomSong();
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
