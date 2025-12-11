using UnityEngine;

public class Obsticle_Gletscheis : MonoBehaviour
{
    [Header("Settings")]
    public float slowMultiplier = 1.5f;

    private void OnTriggerEnter(Collider other)
    {
        Player p = other.GetComponent<Player>();
        if (p != null)
        {
            p.environmentSpeedMultiplier = 1f / slowMultiplier;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Player p = other.GetComponent<Player>();
        if (p != null)
        {
            p.environmentSpeedMultiplier = 1f;
        }
    }
}
