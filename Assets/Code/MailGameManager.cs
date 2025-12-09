using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class MailGameManager : MonoBehaviour
{
    public static MailGameManager instance;

    [Header("Game Settings")]
    public float startTime = 180f;
    public int pointsPerLetter = 100;
    public float timePerLetter = 10f;

    [Header("References")]
    public List<Mailbox> allMailboxes = new List<Mailbox>();
    public TMP_Text timerText;
    public TMP_Text scoreText;

    // Runtime 
    private float timeRemaining;
    private int score = 0;
    private bool gameActive = false;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        timeRemaining = startTime;
        gameActive = true;
        
        // Pick the first random mailbox at start of the game
        SpawnLetterAtRandomMailbox();
        UpdateUI();
    }

    void Update()
    {
        if (!gameActive) return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            GameOver();
        }
        
        UpdateTimerUI();
    }

    public void CollectLetter()
    {
        score += pointsPerLetter;
        timeRemaining += timePerLetter;
        
        // Immediately pick a new mailbox
        SpawnLetterAtRandomMailbox();
        
        UpdateUI();
    }

    public void AddBoost(int bonusPoints, float bonusTime)
    {
        score += bonusPoints;
        timeRemaining += bonusTime;
        UpdateUI();
    }

    void SpawnLetterAtRandomMailbox()
    {
        // Find all boxes that don't currently have a leter
        List<Mailbox> emptyBoxes = new List<Mailbox>();
        foreach (Mailbox box in allMailboxes)
        {
            if (!box.HasLetter())
                emptyBoxes.Add(box);
        }

        // Fail-safe, if all are full, just pick any random one
        if (emptyBoxes.Count == 0) emptyBoxes = allMailboxes;

        if (emptyBoxes.Count > 0)
        {
            int randomIndex = Random.Range(0, emptyBoxes.Count);
            emptyBoxes[randomIndex].PlaceLetter();
        }
    }

    void UpdateUI()
    {
        scoreText.text = "Score: " + score;
        UpdateTimerUI();
    }

    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    void GameOver()
    {
        gameActive = false;
        Debug.Log("Game Over!");
    }
}