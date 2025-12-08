using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class DialogScreen : MonoBehaviour
{
    private DialogLine currentDialog;
    private string currentNPCName;
    private Sprite currentNPCPortrait;

    [Header("UI References")]
    public GameObject panel;
    public TMP_Text nameTMP;
    public TMP_Text dialogTMP;
    public UnityEngine.UI.Image portraitImage;
    public GameObject[] choiceButtons;
    public GameObject continueButton;

    [Header("Input")]
    public PlayerInput input;

    public event System.Action<string> onDialogChoice;

    void Start()
    {
        if (portraitImage != null) 
            portraitImage.enabled = false;
    }

    public void ShowDialog(DialogLine dialog, string npcName, Sprite npcPortrait)
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        currentDialog = dialog;
        currentNPCName = npcName;
        currentNPCPortrait = npcPortrait;

        // Handle inventory items
        if (SimpleInventory.instance != null)
        {
            if (dialog.itemToGive != null)
                SimpleInventory.instance.AddItem(dialog.itemToGive);

            if (dialog.itemToTake != null)
                SimpleInventory.instance.RemoveItem(dialog.itemToTake);
        }

        // Update UI
        if (nameTMP != null) 
            nameTMP.text = npcName;

        if (portraitImage != null)
        {
            if (npcPortrait != null)
            {
                portraitImage.sprite = npcPortrait;
                portraitImage.enabled = true;
            }
            else
            {
                portraitImage.enabled = false;
            }
        }

        panel.SetActive(true);
        dialogTMP.text = dialog.dialogText;

        // Setup choice buttons
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            if (i < dialog.choices.Length)
            {
                choiceButtons[i].SetActive(true);
                choiceButtons[i].GetComponentInChildren<TMP_Text>().text = dialog.choices[i].text;
            }
            else
            {
                choiceButtons[i].SetActive(false);
            }
        }

        // Show continue or choice buttons
        if (dialog.choices.Length == 0)
        {
            continueButton.SetActive(true);
            EventSystem.current.SetSelectedGameObject(continueButton);
        }
        else
        {
            continueButton.SetActive(false);
            EventSystem.current.SetSelectedGameObject(choiceButtons[0]);
        }

        if (input != null) 
            input.SwitchCurrentActionMap("UI");
    }

    public void SelectChoice(int index)
    {
        onDialogChoice?.Invoke(currentDialog.choices[index].id);

        if (currentDialog.choices[index].nextLine != null)
        {
            ShowDialog(currentDialog.choices[index].nextLine, currentNPCName, currentNPCPortrait);
        }
        else
        {
            Hide();
        }
    }

    public void Continue()
    {
        if (currentDialog.defaultNextLine != null)
        {
            ShowDialog(currentDialog.defaultNextLine, currentNPCName, currentNPCPortrait);
        }
        else
        {
            Hide();
        }
    }

    public void Hide()
    {
        panel.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        EventSystem.current.SetSelectedGameObject(null);

        if (input != null) 
            input.SwitchCurrentActionMap("Player");
    }
}