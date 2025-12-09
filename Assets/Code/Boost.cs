using UnityEngine;
using System.Collections;

public class Boost : MonoBehaviour
{
    [Header("Settings")]
    public int bonusPoints = 50;
    public float bonusTime = 5f;
    public float respawnTime = 10f;

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