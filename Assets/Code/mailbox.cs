using UnityEngine;

public class Mailbox : Interactable
{
    [Header("Visual")]
    public GameObject letterVisual;
    
    [Header("Effects")]
    public ParticleSystem collectParticles;
    
    private bool hasLetter = false;

    void Start()
    {
        UpdateVisual();
    }

    public void PlaceLetter()
    {
        hasLetter = true;
        UpdateVisual();
    }

    public override void Interact()
    {
        base.Interact();

        if (!hasLetter) return;

        // Collect the letter
        hasLetter = false;
        UpdateVisual();

        if(collectParticles != null) collectParticles.Play();
        
        if (MailGameManager.instance != null)
        {
            MailGameManager.instance.CollectLetter();
        }
    }

    public bool HasLetter()
    {
        return hasLetter;
    }

    void UpdateVisual()
    {
        if (letterVisual != null)
            letterVisual.SetActive(hasLetter);
    }
}