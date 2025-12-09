using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private InputAction move;
    private InputAction interact;
    private InputAction look;
    
    public Transform referenceCamera;

    public PlayerInput input;
    public CharacterController controller;
    public bool canMove = true;
    
    [Header("Player Stats")]
    public float moveSpeed = 5f;
    public float sprintSpeed = 5f;
    public float currentSpeed;
    public float gravity = -9.81f;
    public float yVelocity;
    
    public bool isSprinting = false;


    void Awake()
    {
        move = input.currentActionMap.FindAction("Move");
        look = input.currentActionMap.FindAction("Look");
        interact = input.currentActionMap.FindAction("Interact");
        
        // ------------ ui 
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Start()
    {
        input.actions.Disable();
        input.currentActionMap?.Enable();
    }

    void Update()
    {
        if (canMove)
        {
            Vector2 moveInput = move.ReadValue<Vector2>();
            Debug.Log(moveInput);
            isSprinting = input.actions["Sprint"].ReadValue<float>() > 0.1;
            
            // calculate movement direction based on look direction
            Vector3 forward = Vector3.ProjectOnPlane(referenceCamera.forward, Vector3.up).normalized;
            Vector3 right = Vector3.ProjectOnPlane(referenceCamera.right, Vector3.up).normalized;
            Vector3 direction = forward * moveInput.y + right * moveInput.x;
            
            // smooth movement rotation
            if (direction != Vector3.zero)
                transform.forward = Vector3.Slerp(transform.forward, direction, 10 * Time.deltaTime);
            
            // velocity berechnen
            direction *= isSprinting ? sprintSpeed : moveSpeed;
            yVelocity -= gravity * Time.deltaTime;
            direction.y = yVelocity;
        }
    }
}
