using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
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
    }

    public void StartDialogue(Dialogue dialogue)
    {
        nameText.text = dialogue.speakerName;
        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
            sentences.Enqueue(sentence);

        StartCoroutine(FadeInDialogueBox());
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
        StartCoroutine(FadeOutDialogueBox());
    }

    IEnumerator FadeInDialogueBox()
    {
        Color tempColor = dialogueBox.color;

        while (tempColor.a < 1f)
        {
            tempColor.a += Time.deltaTime;
            dialogueBox.color = tempColor;
            yield return null;
        }
    }

    IEnumerator FadeOutDialogueBox()
    {
        Color tempColor = dialogueBox.color;

        while (tempColor.a > 0f)
        {
            tempColor.a -= Time.deltaTime;
            dialogueBox.color = tempColor;
            yield return null;
        }
    }
}
