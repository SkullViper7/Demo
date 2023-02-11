using FishNet.Connection;
using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder.Shapes;

public class PlayerMovement : NetworkBehaviour
{
    [Header("Player")]
    public float moveSpeed = 6f;
    public float gravity = -9.81f;
    public float jumpHeight = 2.4f;
    public float distanceToGround = 0.4f;

    private InputMaster controls;
    private Vector3 velocity;
    private Vector3 move;
    private CharacterController controller;

    [Header("Ground")]
    public Transform groundCheck;
    public LayerMask groundMask;
    private bool isGrounded;

    private GameObject tpPoint;
    public Bullet bulletScript;
    public ShootingGun sg;

    private void Awake()
    {
        controls = new InputMaster();
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (!base.IsOwner)
        {
            return;
        }

        Grav();
        PlayerMvmnt();
        Jump();
        TPServer();
    }

    [ServerRpc]
    public void TPServer()
    {
        TP();
    }

    //Gravity Physics
    private void Grav()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, distanceToGround, groundMask); //Check if the player is touching ground

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    //Movement inputs
    private void PlayerMvmnt()
    {
        move = controls.Player.Move.ReadValue<Vector2>();

        Vector3 movement = (move.y * transform.forward) + (move.x * transform.right);
        controller.Move(movement * moveSpeed * Time.deltaTime);
    }

    //Jumping inputs
    private void Jump()
    {
        if (controls.Player.Jump.triggered && isGrounded)
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

    //TP inputs
    [ObserversRpc]
    public void TP()
    {
        tpPoint = gameObject.GetComponent<ShootingGun>().currentBullet;
        if (controls.Player.TP.triggered && sg.currentBullet.GetComponent<Bullet>().canTP)
        {
            transform.position = tpPoint.transform.position;
            sg.currentBullet.GetComponent<Bullet>().canTP = true;
            sg.canShoot = true;
            Invoke("DestroyTP", 0.01f);
        }
    }

    private void DestroyTP()
    {
        Destroy(tpPoint);
    }
}
