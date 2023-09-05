using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class medPackPickup : MonoBehaviour
{
    [SerializeField] medPackStats medPack;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && gameManager.instance.playerScript.medPackAmount < gameManager.instance.playerScript.medPackMaxAmount)
        {
            if (Input.GetKey(KeyCode.E))
            {
                gameManager.instance.playerScript.medPackPickup(medPack);
                Destroy(gameObject);
            }
        }
    }
}
