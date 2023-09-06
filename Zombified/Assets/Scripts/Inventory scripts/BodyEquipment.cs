using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyEquipment : MonoBehaviour
{
    public GameObject Tools;
    public bool BodyEquipmentIsClosed;

    void Start()
    {
        BodyEquipmentIsClosed = false;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.B))
        {
            if (BodyEquipmentIsClosed == true)
            {
                Tools.SetActive(true);
                BodyEquipmentIsClosed = false;
            }
            else
            {
                Tools.SetActive(false);
                BodyEquipmentIsClosed = true;
            }
        }
    }


}
