using UnityEngine;

public class ActiveStateLogger : MonoBehaviour
{
    private void OnEnable()
    {
        Debug.Log(gameObject.name + " has been enabled.", this);
    }

    private void OnDisable()
    {
        Debug.Log(gameObject.name + " has been disabled.", this);
    }
}
