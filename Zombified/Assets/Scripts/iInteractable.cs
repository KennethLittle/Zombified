using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//add this script to any interactable you make. this script plays with the playerInteract script.
public interface iInteractable
{
     void Interact();
     string GetInteractText();  
    Transform GetTransform();
}
