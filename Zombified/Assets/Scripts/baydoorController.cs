using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class baydoorController : MonoBehaviour
{
    public float doorRaiseSpeed = 5f;
    public float raiseHeight = 10f;

    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private bool doorOpened = false;
    private NavMeshObstacle navMeshObstacle;

    private void Start()
    {
        initialPosition = transform.position;
        targetPosition = initialPosition + new Vector3(0, raiseHeight, 0);
        navMeshObstacle = GetComponent<NavMeshObstacle>();
    }

    private void Update()
    {
        if (doorOpened)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, doorRaiseSpeed * Time.deltaTime);

            if (transform.position == targetPosition && navMeshObstacle.enabled)
            {
                navMeshObstacle.enabled = false;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, initialPosition, doorRaiseSpeed * Time.deltaTime);

            if (transform.position == initialPosition && !navMeshObstacle.enabled)
            {
                navMeshObstacle.enabled = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            OpenDoor();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            CloseDoor();
        }
    }

    private void OpenDoor()
    {
        doorOpened = true;
    }

    private void CloseDoor()
    {
        doorOpened = false;
    }
}
