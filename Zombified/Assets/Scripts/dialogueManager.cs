using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dialogueManager : MonoBehaviour
{
    public bool inDialogue;
    private Queue<string> sentences;
    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(dialogue dialogue)
    {
        Debug.Log("Start conversation with " + dialogue.name);
        sentences.Clear();
        dialogue.inDialogue = true;
        inDialogue = true;
        foreach (string sentence in dialogue.sentences) 
        {
            sentences.Enqueue(sentence);
        }
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
        Debug.Log(sentence);
    }
    void EndDialogue()//this is not finished and needs to update the bool inDialogue
    {
        Debug.Log("End of convo.");
        //dialogue.inDialogue = true;
        inDialogue = false;
    }

}
