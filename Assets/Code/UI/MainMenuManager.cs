using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject BTNStart;
    public GameObject BTNSettings;
    public GameObject BTNQuit;
    public GameObject BTNQuitYes;
    public GameObject BTNQuitNo;
    public GameObject BTNBackSettings;

    public GameObject SettingsPanel;
    public GameObject QuitConfirmPanel;


    void Start()
    {
        EventSystem.current.SetSelectedGameObject(BTNStart);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OpenSettings()
    {
        if (SettingsPanel != null) 
            SettingsPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(BTNBackSettings);
    }

    public void MainMenuCloseSettings()
    {
        if (SettingsPanel != null)
            SettingsPanel.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(BTNSettings);
    }

    public void TryQuit()
    {
        Debug.Log("WAS IT FINALLY ENOUGH?????");
        if (QuitConfirmPanel != null)
            QuitConfirmPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(BTNQuit);
    }

    public void YesQuit()
    {
        Debug.Log("That's... pretty bright...");
        Application.Quit();
    }

    public void NoQuit()
    {
        Debug.Log("Damn. Thought i... might... die...");
        EventSystem.current.SetSelectedGameObject(null);
        
        QuitConfirmPanel.SetActive(false);
        
        EventSystem.current.SetSelectedGameObject(BTNQuit);
    }
    
}
