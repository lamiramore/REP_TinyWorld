using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class Player : MonoBehaviour
{
    private InputAction move;
    private InputAction interact;
    private InputAction look;
    private InputAction jump;
    private InputAction dash;
    
    public Transform referenceCamera;

    public Animator animator;
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
    
    [Header("Effects")]
    public ParticleSystem dashParticles;
    public ParticleSystem movementParticles;
    public ParticleSystem landingParticles; 
    
    
    public bool isSprinting = false;
    private bool isDashing = false;
    private bool dashOnCooldown = false;
    private float dashTimer = 0f;
    private bool wasGroundedLastFrame = false;

    private float defaultFinalSpeed;
    private Coroutine speedEffectCoroutine;
    
    [Header("Dynamic Modifiers")]
    public float speedModifier = 1f;
    public float environmentSpeedMultiplier = 1f;
    
    //Um Extern auf den Player Movement zu zugreifen /Powerups und Hürden
    public void AddExternalVerticalBoost(float amount)
    {
        yVelocity = amount;  
    }

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

        defaultFinalSpeed = finalSpeed;
    }

    void Update()
    {
        Vector2 moveInput = move.ReadValue<Vector2>();
        bool isMoving = moveInput.sqrMagnitude > 0.1f;
        float acceleration = finalSpeed / accelTime;
        float modifiedFinalSpeed = defaultFinalSpeed * speedModifier;
        finalSpeed = modifiedFinalSpeed;
        float effectiveFinalSpeed = defaultFinalSpeed * environmentSpeedMultiplier;
        finalSpeed = effectiveFinalSpeed;
        float horizontalInfluence = 0.35f;
        
        if (movementParticles != null)
        {
            if (isMoving && controller.isGrounded)
            {
                if (!movementParticles.isPlaying) movementParticles.Play();
            }
            else
            {
                if (movementParticles.isPlaying) movementParticles.Stop();
            }
        }
        
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
                    animator.SetBool("dashing", false);
                    isDashing = false;
                    
                    if (dashParticles != null && dashParticles.isPlaying)
                        dashParticles.Stop();
                    
                    Invoke(nameof(ResetDashCooldown), dashCooldown);
                }

                return; // cancel movement during dash
            }
            
            // movement ------------------------------------------------------------------------------------------------
            if (isMoving)
            {
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
                    transform.forward = Vector3.Slerp(transform.forward, direction, 100 * Time.deltaTime);

                // acceleration
                if (currentSpeed < finalSpeed)
                {
                    currentSpeed = Mathf.MoveTowards(currentSpeed, finalSpeed, acceleration * Time.deltaTime);
                }
                else if (currentSpeed > finalSpeed)
                {
                    currentSpeed = Mathf.MoveTowards(currentSpeed, finalSpeed, (acceleration * 2) * Time.deltaTime);
                }

                // set velocity ----------------------------------------------------------------------------------------
                direction *= currentSpeed;  // movevelocity
                
                float currentGravity = gravity;

                // fastfall --------------------------------------------------------------------------------------------
                if (yVelocity < 0f)
                {
                    currentGravity *= fastFallMultiplier;
                }
                
                yVelocity -= currentGravity * Time.deltaTime;
                direction.y = yVelocity;

                if (controller.Move(direction * Time.deltaTime) == CollisionFlags.Below) yVelocity = -2;
            }
            else currentSpeed = 0f;
            
            // Animator ------------------------------------------------------------------------------------------------
            animator.SetFloat("speed", Mathf.Abs(moveInput.magnitude * currentSpeed));
            animator.SetFloat("yVelocity", yVelocity);
        }
        
        // Landing Particles ----------------------------------------------------------
        if (!wasGroundedLastFrame && controller.isGrounded)
        {
            if (landingParticles != null)
                landingParticles.Play();
        }
        
        wasGroundedLastFrame = controller.isGrounded;
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
        animator.SetBool("dashing", true);
        Debug.Log("Dash started");
        if (dashParticles != null) dashParticles.Play();
        isDashing = true;
        dashOnCooldown = true;
        dashTimer = dashDuration;
    }

    void ResetDashCooldown()
    {
        dashOnCooldown = false;
    }

    public void ApplySpeedModifier(float multiplier, float duration)
    {
        if (speedEffectCoroutine != null) StopCoroutine(speedEffectCoroutine);
        speedEffectCoroutine = StartCoroutine(SpeedRoutine(multiplier, duration));
    }

    IEnumerator SpeedRoutine(float multiplier, float duration)
    {
        finalSpeed = defaultFinalSpeed * multiplier;
        currentSpeed = finalSpeed;

        yield return new WaitForSeconds(duration);

        finalSpeed = defaultFinalSpeed;
        speedEffectCoroutine = null;
    }
}