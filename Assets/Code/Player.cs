using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private InputAction move;
    private InputAction interact;
    private InputAction look;
    private InputAction jump;
    private InputAction dash;
    
    public Transform referenceCamera;

    public PlayerInput input;
    public CharacterController controller;
    public bool canMove = true;
    
    [Header("Player Stats")]
    public float finalSpeed = 40f;
    public float currentSpeed;
    public float accelTime = 2f;
    public float gravity = -9.81f;
    public float yVelocity;
    
    public bool isSprinting = false;


    void Awake()
    {
        move = input.currentActionMap.FindAction("Move");
        look = input.currentActionMap.FindAction("Look");
        interact = input.currentActionMap.FindAction("Interact");
        jump = input.currentActionMap.FindAction("Jump");
        dash = input.currentActionMap.FindAction("Dash");
        
        
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
        Vector2 moveInput = move.ReadValue<Vector2>();
        bool isMoving = moveInput.sqrMagnitude > 0.1f;
        float acceleration = finalSpeed / accelTime;
        
        if (canMove)
        {
            if (isMoving)
            {
                Debug.Log(moveInput);

                // calculate movement direction based on look direction
                Vector3 forward = Vector3.ProjectOnPlane(referenceCamera.forward, Vector3.up).normalized;
                Vector3 right = Vector3.ProjectOnPlane(referenceCamera.right, Vector3.up).normalized;
                Vector3 direction = forward * moveInput.y + right * moveInput.x;

                // smooth movement rotation
                if (direction != Vector3.zero)
                    transform.forward = Vector3.Slerp(transform.forward, direction, 10 * Time.deltaTime);

                // acceleration
                if (currentSpeed < finalSpeed)
                {
                    currentSpeed = Mathf.MoveTowards(currentSpeed, finalSpeed, acceleration * Time.deltaTime);
                }

                // velocity berechnen
                direction *= currentSpeed;
                yVelocity -= gravity * Time.deltaTime;
                direction.y = yVelocity;

                if (controller.Move(direction * Time.deltaTime) == CollisionFlags.Below)
                    yVelocity = -2;
            }
            else currentSpeed = 0f;
        }
    }
}
