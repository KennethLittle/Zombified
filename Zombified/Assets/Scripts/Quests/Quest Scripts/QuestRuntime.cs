using System.Collections.Generic;

public class QuestRuntime
{
    public Quest blueprint; // The original Quest ScriptableObject
    public int currentStepIndex = 0;
    public List<QuestStepRuntime> stepsRuntime = new List<QuestStepRuntime>();

    public QuestRuntime(Quest originalQuest)
    {
        blueprint = originalQuest;
        foreach (var step in originalQuest.questSteps)
        {
            stepsRuntime.Add(new QuestStepRuntime(step));
        }
    }

    public bool IsQuestComplete => currentStepIndex >= stepsRuntime.Count;

    public QuestStepRuntime CurrentStep => stepsRuntime[currentStepIndex];
}
