using UnityEngine;
using UnityEngine.AI;

public class doorController : MonoBehaviour
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
            if (transform.position == targetPosition && navMeshObstacle.enabled)
            {
                navMeshObstacle.enabled = false;
            }
        }
        if (closeDoor)
        {
            transform.position = Vector3.MoveTowards(transform.position, initialPosition, doorRaiseSpeed * Time.deltaTime);
            if (transform.position == initialPosition && !navMeshObstacle.enabled)
            {
                navMeshObstacle.enabled = true;
            }
        }
    }


    public void OpenDoor()
    {
        doorOpened = true;
    }
}
