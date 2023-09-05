using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class playerInteract : MonoBehaviour
{
    //public bool interactPressed;
    void Update()
    {
        //if (!dialogueManager.inDialogue)
        //{

        //}
        if (Input.GetKey(KeyCode.E))
        {
            iInteractable interactable = GetInteractableObject();
            if (interactable != null)
            {               
                interactable.Interact();   
                //interactPressed = true;
            }
        }
    }
    public iInteractable GetInteractableObject()
    {
        List<iInteractable> iInteractablesList = new List<iInteractable>();
        float interactRange = 2f;
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
        foreach (Collider collider in colliderArray)
        {
            if (collider.TryGetComponent(out iInteractable iInteractable))
            {
                iInteractablesList.Add(iInteractable);
            }
        }

        iInteractable closestiInteractable = null;
        foreach (iInteractable Interactable in iInteractablesList)
        {
            if (closestiInteractable == null)
            {
                closestiInteractable = Interactable;
            }
            else
            {
                if (Vector3.Distance(transform.position, Interactable.GetTransform().position) <
                    Vector3.Distance(transform.position, Interactable.GetTransform().position))
                {
                    closestiInteractable = Interactable;
                }
            }
        }

        return closestiInteractable;
    }
}

