using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class escapeZone : MonoBehaviour
{
    public gameManager gameManager;
    public float escapeCountdown = 5.0f;

    private bool countdownStarted = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !countdownStarted)
        {
            countdownStarted = true;
            Invoke("ActivateEscape", escapeCountdown);
        }
    }

    private void ActivateEscape()
    {
        gameManager.instance.escape();
    }
}
