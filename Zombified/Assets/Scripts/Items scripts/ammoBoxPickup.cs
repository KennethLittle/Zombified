using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ammoBoxPickup : MonoBehaviour
{
    [SerializeField] ammoBoxStats ammoBox;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKey(KeyCode.E))
            {
                gameManager.instance.playerScript.ammoBoxPickup(ammoBox);
                Destroy(gameObject);
            }
        }
    }

}
