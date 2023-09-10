using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public class NPCAI : MonoBehaviour
{
    public string npcName;
    public Quest quest;
    public dialogue dialogue;
    public bool hasShop;
    // Add any other shop related variables here

    public TextMeshProUGUI interactionPrompt;  // Assign this in the Unity Inspector

    private bool playerInRange = false;  // Track if player is within interaction range

    private NavMeshAgent agent;
    private Vector3 initialPosition;
    public float roamRange = 5f;  // Radius around the initial position
    private bool isRoaming;
    [SerializeField] Animator anim;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        initialPosition = transform.position;
        StartRoaming();
    }

    private void Update()
    {
        // If the NPC reaches its destination, set a new destination
        if (isRoaming && Vector3.Distance(transform.position, agent.destination) <= agent.stoppingDistance)
        {
            StartRoaming();
        }

        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            InteractWithPlayer();
        }
    }

    private void StartRoaming()
    {
        isRoaming = true;
        Vector3 randomDirection = Random.insideUnitSphere * roamRange;
        randomDirection += initialPosition;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, roamRange, 1);
        Vector3 finalPosition = hit.position;
        agent.destination = finalPosition;
    }


    private void InteractWithPlayer()
    {
        anim.SetTrigger("isInteracting");
        isRoaming = false;
        agent.isStopped = true;  // Stop the NPC

        // Now check if the NPC has a quest, dialogue or shop and handle accordingly
        if (quest != null && !quest.isActive)
        {
            QuestGiver questGiver = GetComponent<QuestGiver>();
            if (questGiver != null)
            {
                questGiver.OpenQuestWindow();
            }
        }
        else if (dialogue != null && !dialogue.inDialogue)
        {
            NPCInteractable npcInteractable = GetComponent<NPCInteractable>();
            if (npcInteractable != null)
            {
                npcInteractable.Interact();
            }
        }
        // Implement shop interaction similarly...
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  // Assuming your player has a "Player" tag
        {
            playerInRange = true;
            interactionPrompt.text = $"Press E to Talk to {npcName}";
            interactionPrompt.gameObject.SetActive(true);  // Show the interaction prompt
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            interactionPrompt.gameObject.SetActive(false);  // Hide the interaction prompt
        }
    }
}
