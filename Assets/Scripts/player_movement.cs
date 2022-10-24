using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_movement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float groundDrag;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool jumpReady;
    bool doubleJumpReady;

    [Header("CheckOnGround")]
    public float playerHeight;
    public LayerMask IsGround;
    bool grounded;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode fowardKey = KeyCode.W;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;
    ConstantForce gravedad;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        gravedad = GetComponent<ConstantForce>();
        rb.freezeRotation = true;
        JumpReset();
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, IsGround);

        PlayerInput();
        SpeedLimit();

        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void PlayerInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        //if (Input.GetKey(fowardKey)) 
        //{
        //    horizontalInput = 1;
        //}
        //else { 0
        //        }

        if(Input.GetKey(jumpKey))
        {
            if (grounded && jumpReady)
            {
                jumpReady = false;
                Jump();
                Invoke(nameof(DoubleJumpReset), jumpCooldown);
                Invoke(nameof(JumpReset), jumpCooldown);
            }
            else if (doubleJumpReady)
            {
                doubleJumpReady = false;
                DoubleJump();
            }
        }

        if (Input.GetKey(KeyCode.Q))
        {
            GSRight();
        }

    }

    private void Move()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed, ForceMode.Force);
        }
        else
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * airMultiplier, ForceMode.Force);
        }
    }

    private void SpeedLimit()
    {
        Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if(flatVelocity.magnitude > moveSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void DoubleJump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void GSRight()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z - 90f);
        gravedad.force = new Vector3(0f, 0f, -25f);
    }

    private void JumpReset()
    {
        jumpReady = true;
    }

    private void DoubleJumpReset()
    {
        doubleJumpReady = true;
    }
}
