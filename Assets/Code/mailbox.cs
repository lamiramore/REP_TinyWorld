using UnityEngine;

public class Mailbox : Interactable
{
    [Header("Visual")]
    public GameObject letterVisual;
    

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