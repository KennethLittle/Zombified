using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractable : MonoBehaviour
{
    [SerializeField] private string interactText;
    private Animator animator;
    public dialogue dialogue;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void Interact()
    {
        FindObjectOfType<dialogueManager>().StartDialogue(dialogue);
        animator.SetTrigger("Talk");
    }

    public string GetInteractText()
    {
        return interactText;
    }
}
