using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SaveUIManager : MonoBehaviour
{
    public GameObject saveFilePrefab; // Drag your Save File Prefab here in the inspector
    public Transform saveFilesParent; // Drag the Content of your Scroll View here
    public InputField renameInputField;

    public GameObject confirmationPanel; // Drag the Panel (Confirmation Dialog) here.
    public Text confirmationText; // Drag the Text component from the Confirmation Dialog here.

    private int pendingSaveSlot = -1;
    private int currentlySelectedSaveSlot = -1;

    public Button newSaveButton;
    public Button loadButton;

    private string selectedSaveFile = null;

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
        foreach (string saveFile in saveFiles)
        {
            GameObject go = Instantiate(saveFilePrefab, saveFilesParent);
            go.GetComponentInChildren<Text>().text = saveFile;

            // Extract the integer save slot from the filename
            string slotString = saveFile.Replace("savegame", "").Replace(".json", "");
            int slot;
            if (int.TryParse(slotString, out slot))
            {
                Button selectButton = go.GetComponent<Button>();
                selectButton.onClick.AddListener(() => SelectSaveFile(slot));
            }
            else
            {
                Debug.LogError("Failed to parse slot from save file name: " + saveFile);
            }
        }
    }

    public void SelectSaveFile(int slot)
    {
        currentlySelectedSaveSlot = slot;
        // You can add code here to visually highlight the selected save file in your UI
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
            renameInputField.gameObject.SetActive(true); // Show the input field
        }
        else
        {
            Debug.LogWarning("No save file selected for renaming!");
        }
    }

    public void FinishRenameProcess()
    {
        string newName = renameInputField.text;  // Get the new name from the input
        SaveManager.Instance.RenameSaveFile(currentlySelectedSaveSlot, newName);
        renameInputField.gameObject.SetActive(false);  // Hide the input field
        PopulateSaveFiles();  // Refresh the list
    }
}
