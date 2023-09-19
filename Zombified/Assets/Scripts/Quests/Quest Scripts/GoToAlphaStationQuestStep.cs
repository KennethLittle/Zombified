using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "New GoToAlphaStationQuestStep", menuName = "Quests/GoToAlphaStationQuestStep")]
public class GoToAlphaStationQuestStep : QuestStep
{
    public override bool CheckCompletion()
    {
        return false;
    }

    public void EnterAlphaStation()
    {
        if (!isCompleted)
        {
            SceneManager.LoadScene("Alpha Stain");
            GameStateManager.instance.ChangeState(GameStateManager.GameState.Playing);
            isCompleted = true;
        }
    }
}
