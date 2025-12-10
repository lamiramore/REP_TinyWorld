using UnityEngine;

public class SpeedBoostPad : MonoBehaviour
{
    [Header("Boost Settings")]
    public float speedMultiplier = 2.0f; 
    public float duration = 2.0f;        

    [Header("Effects")]
    public ParticleSystem activateParticles;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            player.ApplySpeedModifier(speedMultiplier, duration);
            
            if (AudioManager.instance != null)
            {
                AudioManager.instance.PlayBoost();
            }
            
            if (activateParticles != null) activateParticles.Play();
        }
    }
}