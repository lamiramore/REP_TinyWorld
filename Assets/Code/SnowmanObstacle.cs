using UnityEngine;
using System.Collections;

public class SnowmanObstacle : MonoBehaviour
{
    [Header("Obstacle Settings")]
    public float slowMultiplier = 0.5f; 
    public float duration = 1.5f;      
    public float respawnTime = 5.0f;

    [Header("Visuals")]
    public GameObject geometryRoot;     

    [Header("Effects")]
    public ParticleSystem crashParticles;
    
    private Collider myCollider;

    void Awake()
    {
        myCollider = GetComponent<Collider>();
    }

    //triger enter
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            player.ApplySpeedModifier(slowMultiplier, duration);

            if (AudioManager.instance != null)
            {
                AudioManager.instance.PlayCrash();
            }
            
            if (crashParticles != null) crashParticles.Play();
            
            StartCoroutine(RespawnRoutine());
        }
    }

    //respawns
    IEnumerator RespawnRoutine()
    {
        if (geometryRoot != null) geometryRoot.SetActive(false);
        myCollider.enabled = false;

        yield return new WaitForSeconds(respawnTime);

        if (geometryRoot != null) geometryRoot.SetActive(true);
        myCollider.enabled = true;
    }
}