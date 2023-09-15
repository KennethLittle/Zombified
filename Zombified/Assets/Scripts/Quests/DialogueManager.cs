using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;
    public Queue<string> sentences;

    void Start()
    {
        sentences = new Queue<string>();
    }

}
