using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactionRange = 3f;
    public LayerMask interactableLayer;

    private InputAction interact;
    public PlayerInput input;

    void Awake()
    {
        interact = input.currentActionMap.FindAction("Interact");
    }

    void Update()
    {
        if (interact.WasPressedThisFrame())
        {
            TryInteract();
        }
    }

    void TryInteract()
    {
        // Raycast from camera/player / can change to trigger if needed 
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionRange, interactableLayer))
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();
            if (interactable != null)
            {
                interactable.Interact();
            }
        }
    }

    // we love some good visual feedback
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, transform.forward * interactionRange);
    }
}