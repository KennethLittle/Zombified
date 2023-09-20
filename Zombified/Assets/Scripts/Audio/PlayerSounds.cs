using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class PlayerSounds : MonoBehaviour
{
    [Header("Audio Source")]
    public AudioSource LocomotionSource;
    public AudioSource EmoteSource;

    [Header("Audio Clips")]
    public AudioClip[] FootStepSFX;
    public AudioClip[] TakeDamage;
    public AudioClip[] JumpSFX;
    public AudioClip[] LandSFX;
    public AudioClip LowHealth;

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
        AudioFunctionalities.PlayRandomClip(EmoteSource, JumpSFX, PitchRange.x, PitchRange.y);
    }

    public void LandEmote()
    {
        AudioFunctionalities.PlayRandomClip(EmoteSource, LandSFX, PitchRange.x, PitchRange.y);
    }
}
