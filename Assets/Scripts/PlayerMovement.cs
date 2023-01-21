using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player")]
    private InputMaster controls;
    private Vector3 velocity;
    private Vector3 move;
    private CharacterController controller;
    public float moveSpeed = 6f;
    public float gravity = -9.81f;
    public float jumpHeight = 2.4f;
    public float distanceToGround = 0.4f;

    [Header("Ground")]
    public Transform groundCheck;
    public LayerMask groundMask;
    private bool isGrounded;

    private GameObject tp;

    private void Awake()
    {
        controls = new InputMaster();
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Grav();
        PlayerMvmnt();
        Jump();
        TP();
    }

    private void Grav()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, distanceToGround, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void PlayerMvmnt()
    {
        move = controls.Player.Move.ReadValue<Vector2>();

        Vector3 movement = (move.y * transform.forward) + (move.x * transform.right);
        controller.Move(movement * moveSpeed * Time.deltaTime);
    }

    private void Jump()
    {
        if (controls.Player.Jump.triggered)
        {
            velocity.y = jumpHeight;
        }
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void TP()
    {
        tp = GameObject.FindGameObjectWithTag("Bullet");
        if (controls.Player.TP.triggered)
        {
            transform.position = tp.transform.position;
        }
    }
}
