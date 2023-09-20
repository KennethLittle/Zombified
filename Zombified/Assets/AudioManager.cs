using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class AudioManager : MonoBehaviour
{
    [Header("-------Music & Ambi-------")]
    public AudioSource MusicSource1;
    public AudioSource AmbiSource1;

    [Header("-------For Track Fading-------")]
    public AudioSource MusicSource2;
    public AudioSource AmbiSource2;

    [Header("-------All Other Sources-------")]
    public AudioSource SFXSource;

    [Header("-------Audio Arrays-------")]
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
        
        AudioFunctionalities.PlayRandomClip(MusicSource1, HBTracks);
        AudioFunctionalities.PlayRandomClip(AmbiSource1, HBAmbi);
    }

    void PlaySound(AudioSource source, AudioClip clip)
    {
        source.clip = clip;
        source.Play();
    }


    public void ToggleMusic()
    {
        MusicSource1.mute = !MusicSource1.mute;
    }

    public void ToggleAmbi()
    {
        AmbiSource1.mute = !AmbiSource1.mute;
    }

    public void MusicVolume(float volume)
    {
        MusicSource1.volume = volume;
    }

    public void AmbiVolume(float volume)
    {
        AmbiSource1.volume = volume;
    }
    public void ToggleSFX()
    {
        SFXSource.mute = !AmbiSource1.mute;
    }

    public void SFXVolume(float volume)
    {
        SFXSource.volume = volume;
    }
}
