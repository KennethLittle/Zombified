using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;
    public Queue<string> sentences;
    public DialogueManager instance;

    private void Start()
    {
        sentences = new Queue<string>();
        instance = this;
    }

    public void StartDialogue(Dialogue dialogue)
    {
        nameText.text = dialogue.speakerName;
        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
            sentences.Enqueue(sentence);

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0) 
        {
            EndDialogue();
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
        // Logic for when dialogue ends.
        // Maybe hide the dialogue box or signal another system.

    }
}
