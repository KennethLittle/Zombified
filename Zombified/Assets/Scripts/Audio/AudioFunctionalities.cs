using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioFunctionalities
{
    public static void PlayRandomClip(AudioSource source, AudioClip[] clips, float pitchmin = 1f, float pitchmax = 1f)
    {
        source.pitch = Random.Range(pitchmin, pitchmax);
        int randomClip = Random.Range(0, clips.Length);

        //if (clips.Length < 2)
        //{
        //    source.Play();
        //}// checks to make sure there is more then one.


        ////this loop changes the sound. It cant leave the while loop if the clip it assigns is the same as the one playing
        //while (source.clip == clips[randomClip])
        //{
        //    randomClip = Random.Range(0, clips.Length);
        //}
        if (clips.Length == 1)
        {
            randomClip = 0;
        }
        else
        {
            do
            {
                randomClip = Random.Range(0, clips.Length);
            }while (source.clip == clips[randomClip]);
        }
        source.clip = clips[randomClip];
        source.Play();
    }
}
