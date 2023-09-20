using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADS : MonoBehaviour
{
    public Transform normalPosition;  // Position when not aiming down sights
    public Transform adsPosition;     // Position when aiming down sights
    public float lerpSpeed = 5.0f;    // Speed of transition between positions
    public bool isAiming = false;

    private void Update()
    {
        HandleAiming();
    }

    private void HandleAiming()
    {
        if (Input.GetMouseButton(1))  // Assuming right mouse button for aiming
        {
            StartAiming();
        }
        else
        {
            StopAiming();
        }

        // Lerp the weapon position based on whether we are aiming or not
        Transform targetPosition = isAiming ? adsPosition : normalPosition;
        this.transform.position = Vector3.Lerp(this.transform.position, targetPosition.position, Time.deltaTime * lerpSpeed);

        // If using rotations as well, you can add a similar lerp for rotation:
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, targetPosition.rotation, Time.deltaTime * lerpSpeed);
    }

    private void StartAiming()
    {
        isAiming = true;
        // TODO: Insert your animation code here for transitioning to ADS
    }

    private void StopAiming()
    {
        isAiming = false;
        // TODO: Insert your animation code here for transitioning out of ADS
    }
}
