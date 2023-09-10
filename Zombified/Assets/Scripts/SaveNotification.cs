using UnityEngine;
using System.Collections;
using TMPro; 

public class SaveNotification : MonoBehaviour
{
    public GameObject saveIcon;
    public TextMeshProUGUI saveTextbox;  // TextMeshPro variant
    public float displayDuration = 2.0f;  // Duration to display the save notification

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    public void ShowSaveNotification()
    {
        StartCoroutine(DisplayNotification());
    }

    private IEnumerator DisplayNotification()
    {
        saveIcon.SetActive(true);
        saveTextbox.gameObject.SetActive(true);

        yield return new WaitForSeconds(displayDuration);

        saveIcon.SetActive(false);
        saveTextbox.gameObject.SetActive(false);
    }
}
