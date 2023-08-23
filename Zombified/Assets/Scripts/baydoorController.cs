using UnityEngine;
using UnityEngine.AI;

public class baydoorController : MonoBehaviour
{
    public float doorRaiseSpeed = 5f;
    public float raiseHeight = 10f;

    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private bool doorOpened = false;
    private bool closeDoor = false;
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
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f && navMeshObstacle.enabled)
            {
                navMeshObstacle.enabled = false;
            }
        }
        if (closeDoor)
        {
            transform.position = Vector3.MoveTowards(transform.position, initialPosition, doorRaiseSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, initialPosition) < 0.01f)
            {
                if (!navMeshObstacle.enabled)
                {
                    navMeshObstacle.enabled = true;
                }
                closeDoor = false; 
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
        closeDoor = true; 
    }
}
