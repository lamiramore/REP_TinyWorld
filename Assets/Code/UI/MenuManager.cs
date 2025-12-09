using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [Header("References")]
    public Player TinyPlayer;
    public PlayerInput input;
    
    [Header("Pause")]
    public GameObject BTNResume;
    public GameObject BTNSettings;
    public GameObject BTNMainMenu;
    public GameObject BTNBack;
    public GameObject PausePanel;
    public GameObject SettingsPanel;

    private bool isPaused = false;
    private bool inSettings = false;

    
    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame ||
            (Gamepad.current != null && Gamepad.current.startButton.wasPressedThisFrame))
        {
            if (inSettings)
            {
                EventSystem.current.SetSelectedGameObject(null);
                SettingsPanel.SetActive(false);
                inSettings = false;
                EventSystem.current.SetSelectedGameObject(BTNResume);
            }
            else TogglePause();
        }
    }
    
    // PAUSE LOGIC -----------------------------------------------------------------------------------------------------
    
    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
        
        if (isPaused)
        {
            ShowPauseMenu();
        }
        
        PausePanel.SetActive(isPaused);
        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isPaused;
        
        
        //cursor noch visable & locken
    }
    
    public void ShowPauseMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        PausePanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(BTNResume);
    }
    
    // BUTTONS ---------------------------------------------------------------------------------------------------------

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
        
        isPaused = false;
        Time.timeScale = 1f;
        
        PausePanel.SetActive(false);
        SettingsPanel.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        EventSystem.current.SetSelectedGameObject(null);
        SceneManager.LoadScene(0);
        Debug.Log("back to where we came from, right?");
    }

    public void OpenSettings()
    {
        inSettings = true;
        SettingsPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(BTNBack);
    }

    public void CloseSettings()
    {
        inSettings = false;
        SettingsPanel.SetActive(false);
        EventSystem.current.SetSelectedGameObject(BTNSettings);
    }
}

