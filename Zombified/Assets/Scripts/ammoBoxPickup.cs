using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ammoBoxPickup : MonoBehaviour
{
    [SerializeField] ammoBoxStats ammoBox;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.playerScript.ammoBoxPickup(ammoBox);
            Destroy(gameObject);
        }
    }

}
