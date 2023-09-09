using UnityEngine;
using TMPro;

public class LootBox : MonoBehaviour, iInteractable
{
    public TextMeshProUGUI interactTextObject;
    public LootBoxUI lootBoxUI; // Reference to the LootBoxUI script.
    private bool isPlayerInRange = false;
    private const string INTERACT_MESSAGE = "Press E to Open LootBox";

    private void Update()
    {
        Interact();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            ShowInteractText();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            HideInteractText();
        }
    }

    public Transform GetTransform()
    {
        return this.transform;
    }

    public void Interact()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            HideInteractText();
            lootBoxUI.ToggleUI();
        }
    }

    public string GetInteractText()
    {
        return INTERACT_MESSAGE;
    }

    private void ShowInteractText()
    {
        interactTextObject.gameObject.SetActive(true);
        interactTextObject.text = GetInteractText(); // Sets the text content.
    }

    private void HideInteractText()
    {
        interactTextObject.gameObject.SetActive(false);
    }
}
