using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangeText : MonoBehaviour
{
    public TextMeshProUGUI buttonText;
    public string text;
    public string text2;
    private bool toggle;
    // Start is called before the first frame update
    void Start()
    {
        toggle = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewText()
    {
        if (toggle)
        {
            buttonText.text = text;
            toggle = false;
        }
        else
        {
            buttonText.text = text2;
            toggle = true;
        }

    }

}
