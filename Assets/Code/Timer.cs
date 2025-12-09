using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI text;
    
    private bool timerRunning = false;
    public float currentTime;

    void Start()
    {
        UpdateTimerUI();
        StartTimer();
        
    }

    void Update()
    {
        if (!timerRunning) return;
        
        {
            currentTime += Time.deltaTime;
        }
        
        UpdateTimerUI();
    }
    
    // timer funktionen ------------------------------------------------------------------------------------------------

    public void StartTimer()
    {
        timerRunning = true;
    }

    public void StopTimer()
    {
        timerRunning = false;
    }

    // timer ui updaten ------------------------------------------------------------------------------------------------
    public void UpdateTimerUI()
    {
        // in 00:00:00 format
        int minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);
        
        text.text = $"{minutes:00}:{seconds:00.00}";
    }
    
    // current Time vom timer in float speichern -----------------------------------------------------------------------
    public float GetTime()
    {
        return currentTime;
    }
    
}
