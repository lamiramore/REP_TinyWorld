using UnityEngine;
using System.Collections;
public class Powerup_Jump : MonoBehaviour
{
    [Header("Settings")]
    public float boostForce = 10f;  
    public float respawnTime = 5f;
    
    private Collider ObjectCollider;
    private Renderer[] renderers;
    
    private void Awake()
    {
        ObjectCollider = GetComponent<Collider>();
        renderers = GetComponentsInChildren<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Player p = other.GetComponent<Player>();
        if (p != null)
        {
            // Boost auslösen
            p.AddExternalVerticalBoost(boostForce);

            // Respawn starten
            StartCoroutine(RespawnRoutine());
        }
    }

    private IEnumerator RespawnRoutine()
    {
        // deaktivieren
        ObjectCollider.enabled = false;
        foreach (var r in renderers) r.enabled = false;

        // warten
        yield return new WaitForSeconds(respawnTime);

        // wieder aktivieren
        ObjectCollider.enabled = true;
        foreach (var r in renderers) r.enabled = true;
    }
}
