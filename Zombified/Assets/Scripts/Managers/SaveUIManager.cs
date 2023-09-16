using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class SaveUIManager : MonoBehaviour
{
    public GameObject saveFilePrefab; // Drag your Save File Prefab here in the inspector
    public Transform saveFilesParent; // Drag the Content of your Scroll View here
    public TMP_InputField tmpInputField;

    public GameObject confirmationPanel; // Drag the Panel (Confirmation Dialog) here.
    public TextMeshProUGUI confirmationText; // Drag the Text component from the Confirmation Dialog here.

    private int pendingSaveSlot = -1;
    private int currentlySelectedSaveSlot = -1;

    public Button newSaveButton;
    public Button loadButton;
    public Button renameButton;

    private string selectedSaveFile = null;
    private List<GameObject> saveFileGameObjects = new List<GameObject>();


    private void Start()
    {
        PopulateSaveFiles();
    }

    private void PopulateSaveFiles()
    {
        // First clear out any existing list
        foreach (Transform child in saveFilesParent)
        {
            Destroy(child.gameObject);
        }

        List<string> saveFiles = SaveManager.Instance.GetAllSaveFiles();
        saveFileGameObjects.Clear();
        foreach (string saveFile in saveFiles)
        {
            GameObject go = Instantiate(saveFilePrefab, saveFilesParent);
            saveFileGameObjects.Add(go);

            // Extract the integer save slot from the filename
            string slotString = saveFile.Replace("savegame", "").Replace(".json", "");
            if (!int.TryParse(slotString, out int slot))
            {
                Debug.LogError("Failed to parse slot from save file name: " + saveFile);
                continue; // Skip the rest of this iteration
            }

            go.GetComponentInChildren<Text>().text = saveFile;

            TMP_Text textComponent = go.transform.Find("TextComponent").GetComponent<TMP_Text>();
            textComponent.text = saveFile;

            Button selectButton = go.GetComponent<Button>();
            selectButton.onClick.AddListener(() => SelectSaveFile(slot));
        }
    }
    public void SelectSaveFile(int slot)
    {
        currentlySelectedSaveSlot = slot;

        // Reset all save file text to a default color (e.g., white).
        foreach (GameObject go in saveFileGameObjects)
        {
            TMP_Text text = go.GetComponentInChildren<TMP_Text>();
            if (text != null)
            {
                text.color = Color.white;
            }
        }

        // Highlight the selected save file text with a different color (e.g., blue).
        TMP_Text selectedText = saveFileGameObjects[slot].GetComponentInChildren<TMP_Text>();
        if (selectedText != null)
        {
            selectedText.color = Color.blue;
        }
    }

    public void OnLoadButtonPressed()
    {
        if (currentlySelectedSaveSlot != -1)
        {
            SaveManager.Instance.LoadGame(currentlySelectedSaveSlot);
        }
        else
        {
            Debug.LogWarning("No save file selected!");
        }
    }

    public void OnSaveButtonPressed()
    {
        if (currentlySelectedSaveSlot != -1)
        {
            if (SaveManager.Instance.DoesSaveGameExist(currentlySelectedSaveSlot))
            {
                ShowConfirmationDialog("Do you want to overwrite the existing save file?", currentlySelectedSaveSlot);
            }
            else
            {
                SaveManager.Instance.SaveGame(currentlySelectedSaveSlot);
                PopulateSaveFiles();  // Update the file list
            }
        }
        else
        {
            Debug.LogWarning("No save slot selected!");
        }
    }

    public void OnNewSave()
    {
        int slot = SaveManager.Instance.GetNextSaveSlot();

        if (SaveManager.Instance.DoesSaveGameExist(slot))
        {
            ShowConfirmationDialog("A save file already exists in this slot. Do you want to overwrite?", slot);
        }
        else
        {
            SaveManager.Instance.SaveGame(slot);
            PopulateSaveFiles();  // Update the file list
        }
    }

    public void OnConfirmOverwrite()
    {
        if (pendingSaveSlot != -1)
        {
            SaveManager.Instance.SaveGame(pendingSaveSlot);
            PopulateSaveFiles();  // Update the file list
            HideConfirmationDialog();
        }
    }

    public void OnCancelOverwrite()
    {
        HideConfirmationDialog();
    }

    private void ShowConfirmationDialog(string message, int slot)
    {
        confirmationText.text = message;
        confirmationPanel.SetActive(true);
        pendingSaveSlot = slot;
    }

    private void HideConfirmationDialog()
    {
        confirmationPanel.SetActive(false);
        pendingSaveSlot = -1;
    }

    public void StartRenameProcess()
    {
        if (currentlySelectedSaveSlot != -1)
        {
            tmpInputField.gameObject.SetActive(true); // Show the input field
        }
        else
        {
            Debug.LogWarning("No save file selected for renaming!");
        }
    }

    public void FinishRenameProcess()
    {
        string newName = tmpInputField.text;  // Get the new name from the input
        SaveManager.Instance.RenameSaveFile(currentlySelectedSaveSlot, newName);
        tmpInputField.gameObject.SetActive(false);  // Hide the input field
        PopulateSaveFiles();  // Refresh the list
    }
}
