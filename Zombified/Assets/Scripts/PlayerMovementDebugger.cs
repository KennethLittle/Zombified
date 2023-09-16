using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementDebugger : MonoBehaviour
{
    private Vector3 previousPosition;
    private CharacterController characterController;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        previousPosition = transform.position;
    }

    private void Update()
    {
        DetectPositionChange();
        DetectControllerState();
    }

    private void DetectPositionChange()
    {
        if (previousPosition != transform.position)
        {
            Debug.Log("Player has moved! Previous position: " + previousPosition + ", Current position: " + transform.position);
            previousPosition = transform.position;
        }
    }

    private void DetectControllerState()
    {
        if (characterController.isGrounded)
        {
            Debug.Log("Player is grounded.");
        }

        if (characterController.velocity != Vector3.zero)
        {
            Debug.Log("Player has velocity: " + characterController.velocity);
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log("Player has collided with: " + hit.gameObject.name);
    }
}
