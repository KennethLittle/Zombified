using UnityEngine;

[System.Serializable]
public abstract class QuestStep : ScriptableObject
{
    public string description;
    public bool isCompleted;
    public Dialogue stepDialogue;

    public abstract bool CheckCompletion();

    public void TryCompleteStep()
    {
        if (CheckCompletion())
        {
            isCompleted = true;
        }
    }
}
