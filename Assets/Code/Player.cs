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
    public float finalSpeed;
    public float currentSpeed;
    public float jumpStrength;
    public float dashStrength;
    public float dashDuration;
    public float dashCooldown;
    public float accelTime = 2f;
    public float gravity;
    public float fastFallMultiplier;
    public float yVelocity;
    
    public bool isSprinting = false;
    private bool isDashing = false;
    private bool dashOnCooldown = false;
    private float dashTimer = 0f;
    // private float currentDashSpeed = 0f;
    private bool dashFading = false;
    private float dashFadingTimer = 0.2f;
    


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
        float horizontalInfluence = 0.35f;
        
        // movement ----------------------------------------------------------------------------------------------------
        if (canMove)
        {
            JumpAction();
            
            // dashstate -----------------------------------------------------------------------------------------------
            if (isDashing)
            {
                dashTimer -=Time.deltaTime;
                
                Vector3 dashMovement = transform.forward * dashStrength; // movement nach vorne
                dashMovement.y = 0f; // freeze vertical movement
                
                controller.Move(dashMovement * Time.deltaTime);

                if (dashTimer <= 0f)
                {
                    
                    isDashing = false;
                    Invoke(nameof(ResetDashCooldown), dashCooldown);
                }

                return; // cancel movement during dash
            }
            
            // movement ------------------------------------------------------------------------------------------------
            if (isMoving)
            {
                Debug.Log(moveInput);
                
                // dash starten 
                if (dash.WasPressedThisFrame() && !dashOnCooldown)
                {
                  StartDash();
                  return;
                }

                // calculate movement direction based on look direction
                Vector3 forward = Vector3.ProjectOnPlane(referenceCamera.forward, Vector3.up).normalized;
                Vector3 right = Vector3.ProjectOnPlane(referenceCamera.right, Vector3.up).normalized;
                Vector3 direction = forward * moveInput.y + right * (moveInput.x * horizontalInfluence);


                // smooth movement rotation
                if (direction != Vector3.zero)
                    //transform.forward = Vector3.Slerp(transform.forward, direction, 5 * Time.deltaTime);^^
                    transform.forward = Vector3.Slerp(transform.forward, direction, 100 * Time.deltaTime);

                // acceleration
                if (currentSpeed < finalSpeed)
                {
                    currentSpeed = Mathf.MoveTowards(currentSpeed, finalSpeed, acceleration * Time.deltaTime);
                }

                // set velocity ----------------------------------------------------------------------------------------
                direction *= currentSpeed;  // movevelocity
                
                float currentGravity = gravity;

                // fastfall --------------------------------------------------------------------------------------------
                if (yVelocity < 0f)
                {
                    currentGravity *= fastFallMultiplier;
                    Debug.Log("fastfall");
                }
                
                yVelocity -= currentGravity * Time.deltaTime;
                direction.y = yVelocity;

                if (controller.Move(direction * Time.deltaTime) == CollisionFlags.Below) yVelocity = -2;
            }
            else currentSpeed = 0f;
        }
    }
    
    void JumpAction()
    {
        if (controller.isGrounded && jump.WasPressedThisFrame())
        { 
            yVelocity = jumpStrength;
        }
    }

    void StartDash()
    {
        Debug.Log("oops i fartet");
        isDashing = true;
        dashOnCooldown = true;
        dashTimer = dashDuration;
    }

    void ResetDashCooldown()
    {
        
        dashOnCooldown = false;
    }
}

