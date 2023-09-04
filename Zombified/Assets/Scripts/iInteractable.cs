using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface iInteractable
{
     void Interact();
     string GetInteractText();  
    Transform GetTransform();
}
