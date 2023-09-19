using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInputAudio : MonoBehaviour
{
    public AudioSource MouseInputSource;
    public AudioClip MouseInputClip;

    // Start is called before the first frame update
    void Start()
    {
        MouseInputSource.clip = MouseInputClip;
    }

    // Update is called once per frame
    void Update()
    {
        // this is how you make sure the clip is not playing more then once
        if (Input.GetMouseButtonDown(0) && !MouseInputSource.isPlaying)
        {
            MouseInputSource.Play();
        }
    }
}
