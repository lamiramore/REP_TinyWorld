using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Sources")]
    public AudioSource musicSource;   // Mussic, source wiht 'Loop' checked
    public AudioSource sfxSource;     // sfx one shot
    public AudioSource movementSource; // footsteps source

    [Header("Clips - Atmosphere")]
    public AudioClip backgroundMusic;
    
    [Header("Clips - SFX")]
    public AudioClip collectClip;
    public AudioClip boostClip;
    public AudioClip footstepClip;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // cozy tunes
        if (backgroundMusic != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.Play();
        }
    }

    public void PlayCollect()
    {
        if (collectClip != null) 
            sfxSource.PlayOneShot(collectClip);
    }

    public void PlayBoost()
    {
        if (boostClip != null) 
            sfxSource.PlayOneShot(boostClip);
    }

    // important, call this from Player Update()
    public void HandleFootsteps(bool isMoving, float currentSpeed)
    {
        // so it only triggers when moving fast enough
        if (isMoving && currentSpeed > 0.5f)
        {
            if (!movementSource.isPlaying)
            {
                movementSource.clip = footstepClip;
                movementSource.Play();
            }
            
            // Slight pitch variation based on speed
            movementSource.pitch = 0.9f + (currentSpeed / 20f); 
        }
        else
        {
            if (movementSource.isPlaying)
            {
                movementSource.Stop();
            }
        }
    }
}