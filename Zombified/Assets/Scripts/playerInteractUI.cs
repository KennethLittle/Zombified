using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class playerInteractUI : MonoBehaviour
{
    [SerializeField] private GameObject containerGameObject;
    [SerializeField] private playerInteract playerInteract;
    [SerializeField] private TextMeshProUGUI interactTextMeshProUGUI;

    //public static playerInteractUI instance;
    private void Start()
    {
        this.gameObject.SetActive(false);
    }
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
            //this made the interact ui hide once the player pressed interact but i dont like it...

        //    if (playerInteract.interactPressed == true)
        //{
        //    Hide();
        //}   
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
