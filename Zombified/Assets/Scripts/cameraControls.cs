using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class cameraControls : MonoBehaviour
{

    //[SerializeField] int sensitivity;
    [Range(50, 150)]
    public int sensitivity;

    [SerializeField] int lockVertMin;
    [SerializeField] int lockVertMax;

    [SerializeField] bool invertY;

    float xRotation;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        MouseSensitivity(100);
    }


    void Update()
    {
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensitivity; //When removed Time.dt it makes screen unlocked with paused screen active
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensitivity;

        if (invertY)
            xRotation += mouseY;
        else
            xRotation -= mouseY;

        // clamp the camera rotation on the X-Axis
        xRotation = Mathf.Clamp(xRotation, lockVertMin, lockVertMax);

        //rotate the camera on the x-axis
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        //rotate the player on the y-axis
        transform.parent.Rotate(Vector3.up * mouseX);

    }

    public void MouseSensitivity(int value)
    {
        sensitivity = value;
    }
}
