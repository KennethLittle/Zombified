using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class DoorInteraction : MonoBehaviour
{
    private bool isPlayerClose = false;
    public TextMeshProUGUI promptText;

    private void Start()
    {
        promptText.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (isPlayerClose && Input.GetKeyDown(KeyCode.E))
        {
            LoadNextScene();
        }
    }

    private void LoadNextScene()
    {
        // Load the next scene based on build index
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerClose = true;
            promptText.text = "Press E to go to Alpha Station";
            promptText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerClose = false;
            promptText.gameObject.SetActive(false);
        }
    }
}
