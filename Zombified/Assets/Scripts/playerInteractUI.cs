using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class playerInteractUI : MonoBehaviour
{
    [SerializeField] private GameObject containerGameObject;
    [SerializeField] private playerInteract playerInteract;
    [SerializeField] private TextMeshProUGUI interactTextMeshProUGUI;


    private void Update()
    {

            if (playerInteract.GetInteractableObject() != null)
            {
                Show(playerInteract.GetInteractableObject());
            }
            else
            {
                Hide();
            }
            if (gameManager.instance.isPaused == true)
                {
                    Hide();
                }
        
    }
    private void Show(iInteractable Interactable)
    {
        containerGameObject.SetActive(true);
        interactTextMeshProUGUI.text = Interactable.GetInteractText();
    }
    public void Hide()
    {
        containerGameObject.SetActive(false);
    }
}
