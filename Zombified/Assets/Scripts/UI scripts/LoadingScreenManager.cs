using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreenManager : MonoBehaviour
{
    public Image panel;
    public static LoadingScreenManager Instance;
    [SerializeField] GameObject loadingScreenUI;
    public float displayDuration = 2.0f;  // Duration for which loading screen will be displayed

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SetAlpha(0f);
        if (loadingScreenUI != null)
        {
            loadingScreenUI.SetActive(false);  // Ensure the UI is hidden on start
        }
    }

    public void StartLoadingSequence(int sceneIndex, int saveSlot)
    {
        StartCoroutine(DisplayLoadingScreenThenLoad(sceneIndex, saveSlot));
    }

    private IEnumerator DisplayLoadingScreenThenLoad(int sceneIndex, int saveSlot)
    {
        UIManager.Instance.ToggleUI(false);
        SetAlpha(1f);
        Debug.Log("UI off and alpha set to 1.");

        // Display loading screen
        ShowLoadingScreen();
        Debug.Log("Loading screen displayed.");

        // Wait for the duration
        // yield return new WaitForSeconds(displayDuration);
        yield return null;
        Debug.Log("Waited for duration.");

        // Load the scene
        SceneManager.LoadScene(sceneIndex);
        Debug.Log("Scene loaded.");

        UIManager.Instance.ToggleUI(true);
        Debug.Log("UI on.");

        // Hide the loading screen
        HideLoadingScreen();
        Debug.Log("Loading screen hidden.");
        SetAlpha(0f);
        Debug.Log("Alpha set to 0.");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UIManager.Instance.ToggleUI(true);

        // Hide the loading screen
        HideLoadingScreen();
        SetAlpha(0f);

        // Unregister the event to avoid multiple registrations.
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void SetAlpha(float alpha)
    {
        Color color = panel.color;
        color.a = alpha;
        panel.color = color;
    }

    public void ShowLoadingScreen()
    {
        if (loadingScreenUI != null)
        {
            loadingScreenUI.SetActive(true);
        }
    }

    public void HideLoadingScreen()
    {
        if (loadingScreenUI != null)
        {
            loadingScreenUI.SetActive(false);
        }
    }
}