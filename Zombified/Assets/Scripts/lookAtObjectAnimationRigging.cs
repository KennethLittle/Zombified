using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

//This script is to add a range to how far away you are that NPC's will head track. Needs to be finished.
public class lookAtObjectAnimationRigging : MonoBehaviour
{

    private Rig rig;
    private float targetWeight;

    private void Awake()
    {
        GetComponent<Rig>();
    }

    private void Update()
    {
        rig.weight = Mathf.Lerp(rig.weight, targetWeight, Time.deltaTime * 10f);

        //if ()
    }
}
