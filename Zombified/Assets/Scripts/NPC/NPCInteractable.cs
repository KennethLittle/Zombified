using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractable : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Interact()
    {
        animator.SetTrigger("Talk");
        Debug.Log("Interact");
    }
}
