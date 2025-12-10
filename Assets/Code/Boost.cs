using UnityEngine;
using System.Collections;
// THIS IS THE "COIN"
public class Boost : MonoBehaviour
{
    [Header("Settings")]
    public int bonusPoints = 50;
    public float bonusTime = 5f;
    public float respawnTime = 10f;

    [Header("Effects")]
    public ParticleSystem collectParticles;
    
    private Collider myCollider;
    private Renderer myRenderer;

    void Awake()
    {
        myCollider = GetComponent<Collider>();
        myRenderer = GetComponent<Renderer>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (MailGameManager.instance != null)
            {
                MailGameManager.instance.AddBoost(bonusPoints, bonusTime);
                
                if (AudioManager.instance != null) 
                    AudioManager.instance.PlayBoost();

                if(collectParticles != null) collectParticles.Play();
                
                StartCoroutine(RespawnRoutine());
            }
        }
    }

    IEnumerator RespawnRoutine()
    {
        myCollider.enabled = false;
        if(myRenderer != null) myRenderer.enabled = false;

        yield return new WaitForSeconds(respawnTime);

        myCollider.enabled = true;
        if(myRenderer != null) myRenderer.enabled = true;
    }
}