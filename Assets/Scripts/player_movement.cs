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
    public float dashForce;
    public float numDashes;
    public float dashCooldown;
    bool jumpReady;
    bool doubleJumpReady;
    bool dashReady;

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

    Vector3 angulo= new Vector3(0f, 0f, 0f);
    Quaternion caida;
    Quaternion inicio;
    public float duracion;
    float ratio;
    float tiempo;

    Rigidbody rb;
    ConstantForce gravedad;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        gravedad = GetComponent<ConstantForce>();
        inicio = transform.rotation;
        ratio = 1.0f / duracion;
        tiempo = 0.0f;
        rb.freezeRotation = true;
        JumpReset();
        DashReset();
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, -transform.up , playerHeight * 0.5f + 0.2f, IsGround);

        PlayerInput();
        SpeedLimit();

        if (grounded)
        {
            rb.drag = groundDrag;
            doubleJumpReady = false;
        }
        else
        {
            rb.drag = 0;
        }

        if (tiempo < 1.0)
        {
            tiempo += Time.deltaTime * ratio;
            transform.rotation = Quaternion.Slerp(inicio, caida, tiempo);
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

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if(numDashes > 0 && dashReady)
            {
                dashReady = false;
                numDashes -= 1;
                moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
                Dash(moveDirection);
                Invoke(nameof(DashRecover), dashCooldown);
                Invoke(nameof(DashReset), 0.25f);
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

    public void Dash(Vector3 moveDirection)
    {
        rb.AddForce(moveDirection.normalized * jumpForce, ForceMode.Impulse);
    }

    private void GSRight()
    {
        //transform.rotation = Quaternion.Slerp(new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w), 
        //    new Quaternion(transform.rotation.x + 0.5f, transform.rotation.y, transform.rotation.z,transform.rotation.w), 0.02f);
        caida = inicio * Quaternion.Euler(new Vector3(90f, 0, 0));
        tiempo = 0.0f;
    }

    private void JumpReset()
    {
        jumpReady = true;
    }

    private void DoubleJumpReset()
    {
        doubleJumpReady = true;
    }

    private void DashRecover()
    {
        numDashes += 1;
    }
    private void DashReset()
    {
        dashReady = true;
    }
}
