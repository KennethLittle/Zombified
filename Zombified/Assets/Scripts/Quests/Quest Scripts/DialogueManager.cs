using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI continuePrompt;
    public Image dialogueBox;
    public Queue<string> sentences;
    public static DialogueManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        sentences = new Queue<string>();
        instance = this;
        continuePrompt.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) 
        {
            DisplayNextSentence();
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {

        nameText.text = dialogue.speakerName;
        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
            sentences.Enqueue(sentence);
        Debug.Log("Starting dialogue: " + dialogue.sentences);
        UIManager.Instance.showDialogueBox();
        DisplayNextSentence();
        continuePrompt.gameObject.SetActive(true);
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            continuePrompt.gameObject.SetActive(false);


            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines(); // Ensure the text isn't still typing out when we get the next sentence
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null; // This gives the typewriter effect. Adjust as needed.
        }
    }

    void EndDialogue()
    {
        UIManager.Instance.hideDialogueBox();
    }

    
    IEnumerator WaitAndEndDialogue(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        EndDialogue();
    }
}
