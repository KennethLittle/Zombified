using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    [SerializeField][Range(0, 1)] float Rate;
    [SerializeField][Range(0, 1)] float Volume;
}
