using UnityEngine;
using System.Collections;

public class SnowmanFall : MonoBehaviour
{
    [Header("Player Detection")]
    public Transform player;
    public float triggerDistance = 5f;
    public string animatorTrigger = "Fall";
    public string StandTrigger = "Stand";

    [Header("Fall Settings")]
    public Collider boxCol;
    public float fallAngle = 90f;         
    public float sideTiltAngle = 15f;   
    public float fallTime = 0.5f;

    [Header("Reset Settings")]
    public float resetDelay = 5f;
    public float cooldown = 30f;
    private bool canTrigger = true;

    private Animator anim;
    private Quaternion startRot;
    private Quaternion fallRot;

    void Start()
    {
        anim = GetComponent<Animator>();

        if (boxCol == null)
            boxCol = GetComponent<Collider>();

        startRot = boxCol.transform.localRotation;

        fallRot = Quaternion.Euler(
            startRot.eulerAngles.x + fallAngle,
            startRot.eulerAngles.y + sideTiltAngle,  
            startRot.eulerAngles.z
        );
    }

    void Update()
    {
        if (!canTrigger) return;

        if (Vector3.Distance(player.position, transform.position) < triggerDistance)
        {
            anim.SetTrigger(animatorTrigger);
            StartCoroutine(FallRoutine());
        }
    }

    IEnumerator FallRoutine()
    {
        canTrigger = false;
        
        yield return StartCoroutine(RotateCollider(boxCol.transform, startRot, fallRot, fallTime));
        
        yield return new WaitForSeconds(resetDelay);
        
        anim.SetTrigger(StandTrigger);
        
        yield return StartCoroutine(RotateCollider(boxCol.transform, fallRot, startRot, fallTime));
        
        yield return new WaitForSeconds(cooldown);

        canTrigger = true;
    }

    IEnumerator RotateCollider(Transform target, Quaternion from, Quaternion to, float duration)
    {
        float t = 0;
        while (t < duration)
        {
            target.localRotation = Quaternion.Lerp(from, to, t / duration);
            t += Time.deltaTime;
            yield return null;
        }
        target.localRotation = to;
    }
}