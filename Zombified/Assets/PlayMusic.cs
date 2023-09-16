using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusic : MonoBehaviour
{
    void Start()
    {
        AudioManager.instance.PlayMusic("MainMenu");
    }

}
