using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DoorToAlphaStation : MonoBehaviour
{
    private bool playerNearby = false;
    public TextMeshProUGUI doorInteraction;

    private void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            // Notify the quest system first
            QuestManager.instance.NotifyGotToAlphaStation();

            // Check if there is a relevant quest to go to Alpha Station
            if (QuestManager.instance.CurrentQuest == null)
            {
                Debug.Log("No quest available. Transitioning...");
                GoToAlphaStation();
            }
            else if (QuestManager.instance.CurrentQuest.CurrentStep.isCompleted)
            {
                Debug.Log("Quest step is completed. Transitioning...");
                GoToAlphaStation();
            }
            else
            {
                Debug.Log("Quest step is not completed. Can't transition.");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
            doorInteraction.text = "Press E to go to Alpha Station";
            doorInteraction.gameObject.SetActive(true);  // Make the TextMeshPro object visible
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
            doorInteraction.gameObject.SetActive(false);  // Hide the TextMeshPro object
        }
    }

    private void GoToAlphaStation()
    {
        PlayerData data = new PlayerData(PlayerManager.instance);
        PlayerManager.TempPlayerData = data;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
