using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSwap2 : MonoBehaviour
{
    public AudioManager AudioSource;
    private bool outside = true;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopAllCoroutines();
            StartCoroutine(FadeTrack());
        }
    }
    private IEnumerator FadeTrack()
    {
        float timeToFade = 0.25f;
        float timeElapsed = 0;
        if (outside)
        {
            AudioFunctionalities.PlayRandomClip(AudioSource.MusicSource2, AudioSource.ASTracks);
            AudioFunctionalities.PlayRandomClip(AudioSource.AmbiSource2, AudioSource.ASAmbi);
            while (timeElapsed < timeToFade)
            {
                AudioSource.MusicSource2.volume = Mathf.Lerp(0, 1, timeElapsed / timeToFade);
                AudioSource.MusicSource1.volume = Mathf.Lerp(1, 0, timeElapsed / timeToFade);
                timeElapsed += Time.deltaTime;
                yield return new WaitForSeconds(.5f);
            }
            AudioSource.MusicSource1.Stop();
            AudioSource.AmbiSource1.Stop();
        }
        else
        {
            AudioFunctionalities.PlayRandomClip(AudioSource.MusicSource1, AudioSource.OSTracks);
            AudioFunctionalities.PlayRandomClip(AudioSource.AmbiSource1, AudioSource.OSAmbi);
            while (timeElapsed < timeToFade)
            {
                AudioSource.MusicSource1.volume = Mathf.Lerp(0, 1, timeElapsed / timeToFade);
                AudioSource.MusicSource2.volume = Mathf.Lerp(1, 0, timeElapsed / timeToFade);
                timeElapsed += Time.deltaTime;
                yield return new WaitForSeconds(.5f);
            }
            AudioSource.MusicSource2.Stop();
            AudioSource.AmbiSource2.Stop();
        }
        outside = false;
    }
}
