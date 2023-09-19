using UnityEngine;

public abstract class QuestStep : ScriptableObject
{
    public string description;
    public bool isCompleted;
    public Dialogue stepDialogue;
    public int stepID;

    public abstract bool CheckCompletion();
}