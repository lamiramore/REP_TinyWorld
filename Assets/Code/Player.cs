using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private InputAction moveAction;
    private InputAction look;
    private Transform referenceCamera;

    public PlayerInput input;
    public CharacterController controller;
    public bool canMove;
    
    [Header("Player Stats")]
    public float moveSpeed = 5f;
    public float sprintSpeed = 5f;
    public float currentSpeed;
    public float gravity = -9.81f;
    public float yVelocity;
    
    public bool isSprinting = false;


    void Awake()
    {
        moveAction = input.currentActionMap.FindAction("Move");
        look = input.currentActionMap.FindAction("Look");
    }

    void Update()
    {
        if (canMove)
        {
            Vector2 moveInput = moveAction.ReadValue<Vector2>();
            isSprinting = input.actions["Sprint"].ReadValue<float>() > 0.1;
        }
    }
}
