using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource MusicSource;
    public AudioSource AmbiSource;
    public AudioSource SFXSource;
    public AudioClip[] HBTracks;
    public AudioClip[] HBAmbi;
    public AudioClip[] OSTracks;
    public AudioClip[] OSAmbi;
    public AudioClip[] ASTracks;
    public AudioClip[] ASAmbi;

    private bool inHomeBase;
    private bool inOutDoors;
    private bool inAlphaStain;

    void Start()
    {
        PlayHomeBase();


    }
    void PlaySound(AudioSource source, AudioClip clip)
    {
        source.clip = clip;
        source.Play();
    }

    //public void SwapTrack(AudioClip[] newMusic, AudioClip[] newAmbi)
    //{

    //}
    public void PlayHomeBase()
    {
        AudioFunctionalities.PlayRandomClip(MusicSource, HBTracks);
        AudioFunctionalities.PlayRandomClip(AmbiSource, HBAmbi);
    }
    public void PlayOutSide()
    {
        AudioFunctionalities.PlayRandomClip(MusicSource, OSTracks);
        AudioFunctionalities.PlayRandomClip(AmbiSource, OSAmbi);
    }


    public void ToggleMusic()
    {
        MusicSource.mute = !MusicSource.mute;
    }

    public void ToggleAmbi()
    {
        AmbiSource.mute = !AmbiSource.mute;
    }

    public void MusicVolume(float volume)
    {
        MusicSource.volume = volume;
    }

    public void AmbiVolume(float volume)
    {
        AmbiSource.volume = volume;
    }
    public void ToggleSFX()
    {
        SFXSource.mute = !AmbiSource.mute;
    }

    public void SFXVolume(float volume)
    {
        SFXSource.volume = volume;
    }
}
