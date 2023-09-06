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
        DisplayNextSentence(dialogue);
    }


    public void DisplayNextSentence(dialogue dialogue)
    {
        if (sentences.Count == 0) 
        {
            EndDialogue(dialogue);
            return;
        }

        string sentence = sentences.Dequeue();
        Debug.Log(sentence);
    }
    void EndDialogue(dialogue dialogue)//this is not finished and needs to update the bool inDialogue
    {
        Debug.Log("End of convo.");
        dialogue.inDialogue = false;
        inDialogue = false;
    }

}
