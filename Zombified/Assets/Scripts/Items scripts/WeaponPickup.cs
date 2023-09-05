using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] WeaponStats gun;
    

    void Start()
    {
        gun.ammoCur = gun.ammoMax;
    }

    private void OnTriggerStay(Collider other)
    {
            if (other.CompareTag("Player"))
            {
             if (Input.GetKey(KeyCode.E))
             {
                gameManager.instance.playerScript.weaponpickup(gun);
                Destroy(gameObject);
             }
            }
    }

}
