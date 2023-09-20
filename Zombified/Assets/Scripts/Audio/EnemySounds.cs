using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySounds : MonoBehaviour
{
    [Header("Audio Source")]
    public AudioSource LocomotionSource;
    public AudioSource EmoteSource;

    [Header("Audio Clips")]
    public AudioClip[] FootStepSFX;

    public void PlayEnemyFootsteps()
    {
        AudioFunctionalities.PlayRandomClip(LocomotionSource, FootStepSFX);
    }
}
