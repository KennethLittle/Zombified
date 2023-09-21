using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;
using System;
using System.Threading.Tasks;

public class PlayerSounds : MonoBehaviour
{
    [Header("Audio Source")]
    public AudioSource LocomotionSource;
    public AudioSource JumpSource;
    public AudioSource DamageSource;
    public AudioSource LowHealthSource;
    public AudioSource ShootSource;

    [Header("Audio Clips")]
    public AudioClip[] FootStepSFX;
    public AudioClip[] TakeDamageSFX;
    public AudioClip[] JumpSFX;
    public AudioClip[] LandSFX;
    public AudioClip[] LowHealthSFX;
    public AudioClip[] ShootSFX;

    [Range(0, 1)]
    public float FootstepFrequency;
    public Vector2 PitchRange = new Vector2(0.9f, 1.1f);

    [Range(0, 1)]
    public float _footstepDistanceCounter;


    public void PlayFootstep(Vector3 velocity)
    {

        if (_footstepDistanceCounter >= 1f / FootstepFrequency)
        {
            _footstepDistanceCounter = 0f;

            AudioFunctionalities.PlayRandomClip(LocomotionSource, FootStepSFX, PitchRange.x, PitchRange.y);

        }

        _footstepDistanceCounter += velocity.magnitude * Time.deltaTime;
    }

    public void JumpEmote()
    {
        AudioFunctionalities.PlayRandomClip(JumpSource, JumpSFX, PitchRange.x, PitchRange.y);
    }

    public void LandEmote()
    {
        AudioFunctionalities.PlayRandomClip(JumpSource, LandSFX, PitchRange.x, PitchRange.y);
    }

    public void LowHealthEmote()
    {
        AudioFunctionalities.PlayRandomClip(LowHealthSource, LowHealthSFX, PitchRange.x, PitchRange.y);
    }

    public void TakeDamageEmote()
    {
        AudioFunctionalities.PlayRandomClip(DamageSource, TakeDamageSFX, PitchRange.x, PitchRange.y);
    }
    public void ShootEmote()
    {
        AudioFunctionalities.PlayRandomClip(ShootSource, ShootSFX, PitchRange.x, PitchRange.y);
    }    
}
