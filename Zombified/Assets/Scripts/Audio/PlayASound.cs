using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayASound : MonoBehaviour
{
    public AudioSource MusicSource;
    public AudioClip[] MusicClips;
    public AudioSource AmbiSource;
    public AudioClip AmbiClip;

    // Start is called before the first frame update
    void Start()
    {
        //PlaySound(MusicSource, MusicClip);
        AudioFunctionalities.PlayRandomClip(MusicSource, MusicClips);
        PlaySound(AmbiSource, AmbiClip);

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
}
