using UnityEngine;
using System.Collections;
using TMPro;
using static Unity.VisualScripting.Member;

public class RegularDoor : MonoBehaviour
{
    private bool playerNearby = false;
    private bool doorOpen = false;
    public TextMeshProUGUI doorInteraction;

    public AudioSource doorSource;
    public AudioClip doorClip;

    [SerializeField]
    private float doorOpenHeight = 3.0f; // The height by which the door will open
    [SerializeField]
    private float doorOpenSpeed = 1.0f;  // Speed at which the door will open

    private Vector3 closedPosition;
    private Vector3 openPosition;

    private void Start()
    {
        closedPosition = transform.position;
        openPosition = closedPosition + new Vector3(0, doorOpenHeight, 0);
    }

    private void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            ToggleDoor();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
            doorInteraction.text = "Press E to open door";
            doorInteraction.gameObject.SetActive(true);  // Make the TextMeshPro object visible
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
            doorInteraction.gameObject.SetActive(false);  // Hide the TextMeshPro object
        }
    }

    private void ToggleDoor()
    {
        doorOpen = !doorOpen;
        DoorSound();
        StopAllCoroutines();
        if (doorOpen)
        {
            StartCoroutine(OpenDoor());
        }
        else
        {
            StartCoroutine(CloseDoor());
        }
    }

    private void DoorSound()
    {
        doorSource.clip = doorClip;
        doorSource.Play();
    }

    private IEnumerator OpenDoor()
    {
        while (Vector3.Distance(transform.position, openPosition) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, openPosition, doorOpenSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator CloseDoor()
    {
        while (Vector3.Distance(transform.position, closedPosition) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, closedPosition, doorOpenSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
