using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnHomeQuestStep : QuestStep
{
    public override bool CheckCompletion()
    {
        return false;
    }

    public void ReturnHome()
    {
        if (!isCompleted)
        {
            SceneManager.LoadScene("HomeBase");
            isCompleted = true;
        }
    }
}
