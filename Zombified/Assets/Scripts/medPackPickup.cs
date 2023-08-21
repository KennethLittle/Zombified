using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class medPackPickup : MonoBehaviour
{
    [SerializeField] medPackStats medPack;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && gameManager.instance.playerScript.medPackAmount < gameManager.instance.playerScript.medPackMaxAmount)
        {
            gameManager.instance.playerScript.medPackPickup(medPack);
            Destroy(gameObject);
        }
    }
}
