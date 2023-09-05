using UnityEngine;
using System;

public class baydoorController : MonoBehaviour
{
    public float doorRaiseSpeed = 5f;
    public float raiseHeight = 10f;

    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private bool closeDoor = false;

    // Counter to keep track of how many entities are within the trigger zone
    private int entitiesInTriggerZone = 0;

    private void Start()
    {
        initialPosition = transform.position;
        targetPosition = initialPosition + new Vector3(0, raiseHeight, 0);
    }

    private void Update()
    {
        if (entitiesInTriggerZone > 0)
        {
            // Open the door
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, doorRaiseSpeed * Time.deltaTime);
        }
        else if (closeDoor)
        {
            // Close the door
            transform.position = Vector3.MoveTowards(transform.position, initialPosition, doorRaiseSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, initialPosition) < 0.01f)
            {
                closeDoor = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            entitiesInTriggerZone++; // Increment counter
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            entitiesInTriggerZone--; // Decrement counter

            if (entitiesInTriggerZone <= 0)
            {
                entitiesInTriggerZone = 0; // Make sure it doesn't go below 0
                closeDoor = true; // Close the door only if counter is 0
            }
        }
    }
}
