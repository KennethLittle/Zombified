using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSwap : MonoBehaviour
{
    public AudioManager AudioSource;
    private bool inside = true;
    private void OnTriggerEnter(Collider other)
    {
        //if (inside == true)
        //    inside = false;
        //else if (inside == false)
        //    inside = true;

        if (other.CompareTag("Player"))
        {
            if (inside)
            {
                inside = false;
                AudioSource.MusicSource.Stop();
                AudioSource.AmbiSource.Stop();
                AudioSource.PlayOutSide();
            }
            else
            {
                inside = true;
                AudioSource.MusicSource.Stop();
                AudioSource.AmbiSource.Stop();
                AudioSource.PlayHomeBase();
            }

        }
    }
}


