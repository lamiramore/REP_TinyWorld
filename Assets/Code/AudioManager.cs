using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioSource movementSource;

    [Header("Clips - Atmosphere")]
    public AudioClip backgroundMusic;
    
    [Header("Clips - SFX")]
    public AudioClip collectClip;    
    public AudioClip boostClip;     
    public AudioClip speedPadClip;  
    public AudioClip crashClip;      
    public AudioClip footstepClip;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (backgroundMusic != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.Play();
        }
    }

    // picking up a Letter
    public void PlayCollect()
    {
        if (collectClip != null) sfxSource.PlayOneShot(collectClip);
    }

    // Floating Coin/Time Boost
    public void PlayBoost()
    {
        if (boostClip != null) sfxSource.PlayOneShot(boostClip);
    }

    // Speed Pad
    public void PlaySpeedPad()
    {
        if (speedPadClip != null) sfxSource.PlayOneShot(speedPadClip);
    }

    // snowman hit
    public void PlayCrash()
    {
        if (crashClip != null) sfxSource.PlayOneShot(crashClip);
    }

    //footsteps
    public void HandleFootsteps(bool isMoving, float currentSpeed)
    {
        if (isMoving && currentSpeed > 0.5f)
        {
            if (!movementSource.isPlaying)
            {
                movementSource.clip = footstepClip;
                movementSource.Play();
            }
            movementSource.pitch = 0.9f + (currentSpeed / 20f); 
        }
        else
        {
            if (movementSource.isPlaying) movementSource.Stop();
        }
    }
}