using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorInteractable : MonoBehaviour, iInteractable
{
    private Animator animator;
    private bool isOpen;
    public string doorText;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void ToggleDoor()
    {
        isOpen = !isOpen;
        animator.SetBool("IsOpen", isOpen);
    }

    public void Interact()
    {
        ToggleDoor();
    }

    public string GetInteractText()
    {
        return doorText;
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
