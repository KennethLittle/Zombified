using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class playerInteract : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            float interactRange = 2f;
            Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
            foreach (Collider collider in colliderArray)
            {
                if (collider.TryGetComponent(out NPCInteractable npcInteractable))
                {
                    npcInteractable.Interact();

                }
            }
        }
    }
    public NPCInteractable GetInteractableObject()
    {
        List<NPCInteractable> NPCInteractablesList = new List<NPCInteractable>();
        float interactRange = 2f;
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
        foreach (Collider collider in colliderArray)
        {
            if (collider.TryGetComponent(out NPCInteractable NPCInteractable))
            {
                NPCInteractablesList.Add(NPCInteractable);
            }
        }

        NPCInteractable closestNPCInteractable = null;
        foreach (NPCInteractable NPCInteractable in NPCInteractablesList)
        {
            if (closestNPCInteractable == null)
            {
                closestNPCInteractable = NPCInteractable;
            }
            else
            {
                if (Vector3.Distance(transform.position, NPCInteractable.transform.position) <
                    Vector3.Distance(transform.position, closestNPCInteractable.transform.position))
                {
                    closestNPCInteractable = NPCInteractable;
                }
            }
        }

        return closestNPCInteractable;
    }
}

