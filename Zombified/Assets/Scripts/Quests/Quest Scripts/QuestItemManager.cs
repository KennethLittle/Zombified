using UnityEngine;

public class QuestItemManager : MonoBehaviour
{
    public static QuestItemManager instance;

    public GameObject[] questItems;  // Drag your GameObjects here in Unity Editor

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
}
