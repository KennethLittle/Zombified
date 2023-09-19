using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource MusicSource;
    public AudioClip[] MusicClips;
    public AudioSource AmbiSource;
    public AudioClip[] AmbiClips;
    public AudioSource SFXSource;
    public AudioClip[] SFXClips;

    void Start()
    {
        //PlaySound(MusicSource, MusicClip);
        AudioFunctionalities.PlayRandomClip(MusicSource, MusicClips);
        AudioFunctionalities.PlayRandomClip(AmbiSource, AmbiClips);

        //AudioFunctionalities.PlayRandomClip();

    }
    void PlaySound(AudioSource source, AudioClip clip)
    {
        source.clip = clip;
        source.Play();
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
