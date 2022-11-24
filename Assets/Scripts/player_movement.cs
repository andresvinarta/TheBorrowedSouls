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
    public float rotationCooldown;
    public float airMultiplier;
    public float dashForce;
    public float numDashes;
    public float dashCooldown;
    public float slideForce;
    public float slideHeight;
    private float startHeight;
    bool slideStart;
    bool jumpReady;
    bool rotationReady;
    bool doubleJumpReady;
    bool dashReady;
    bool moving;

    [Header("CheckOnGround")]
    public float playerHeight;
    public LayerMask IsGround, IsGroundfrfr;
    bool grounded;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode fowardKey = KeyCode.W;

    [Header("WallRun")]
    public float wallRunForce;
    public float maxWallRunTime;
    public float wallRunTimer;

    [Header("Detection")]
    public float wallCheckDistance;
    public float minJumpHeigth;
    private RaycastHit leftWallhit;
    private RaycastHit rightWallhit;
    private bool wallLeft;
    private bool wallRight;

    public Transform orientation;

    private float horizontalInput;
    private float verticalInput;

    Vector3 moveDirection;

    Vector3 angulo= new Vector3(0f, 0f, 0f);
    Quaternion caida;
    Quaternion inicio;
    public float duracion;
    float ratio;
    float tiempo;

    Camera camaraPrincipal;

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
        rotationReady = true;
        camaraPrincipal = Camera.main;
        startHeight = transform.localScale.y;
        slideStart = true;
        moving = false;
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, -transform.up , playerHeight * 0.5f + 0.2f, IsGround) 
                   || Physics.Raycast(transform.position, -transform.up, playerHeight * 0.5f + 0.2f, IsGroundfrfr);

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

        if (tiempo <= 1.0 && !rotationReady)
        {
            tiempo += Time.deltaTime * ratio;
            transform.rotation = Quaternion.Slerp(inicio, caida, tiempo);
            if (tiempo > 1.0)
            {
                inicio = transform.rotation;
                Invoke(nameof(RotationReset), rotationCooldown);
            }
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

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            transform.localScale = new Vector3(transform.localScale.x, slideHeight, transform.localScale.z);
            rb.AddForce(-transform.up * 5f, ForceMode.Impulse);
            if (slideStart)
            {
                moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
                Slide(moveDirection);
                slideStart = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            transform.localScale = new Vector3(transform.localScale.x, startHeight, transform.localScale.z);
            slideStart = true;
        }

            float rotacion = camaraPrincipal.transform.localEulerAngles.y;
        //Debug.Log(rotacion);
        if (Input.GetKey(KeyCode.Q))
        {
            if (rotacion <= 325f && rotacion >= 235f) GiroPositivoX();
            if (rotacion <= 135f && rotacion >= 45f) GiroNegativoX();
            if (rotacion <= 45f || rotacion >= 325f) GiroNegativoZ();
            if (rotacion <= 235f && rotacion >= 135f) GiroPositivoZ();
        }
        if (Input.GetKey(KeyCode.E))
        {
            if (rotacion <= 325f && rotacion >= 235f) GiroNegativoX();
            if (rotacion <= 135f && rotacion >= 45f) GiroPositivoX();
            if (rotacion <= 45f || rotacion >= 325f) GiroPositivoZ();
            if (rotacion <= 235f && rotacion >= 135f) GiroNegativoZ();

        }
        if (Input.GetKey(KeyCode.F))
        {
            if (rotacion <= 325f && rotacion >= 235f) GiroNegativoZ();
            if (rotacion <= 135f && rotacion >= 45f) GiroPositivoZ();
            if (rotacion <= 45f || rotacion >= 325f) GiroNegativoX();
            if (rotacion <= 235f && rotacion >= 135f) GiroPositivoX();
        }
        if (Input.GetKey(KeyCode.C))
        {
            if (rotacion <= 325f && rotacion >= 235f) GiroPositivoZ();
            if (rotacion <= 135f && rotacion >= 45f) GiroNegativoZ();
            if (rotacion <= 45f || rotacion >= 325f) GiroNegativoX();
            if (rotacion <= 235f && rotacion >= 135f) GiroPositivoX();
        }

    }

    private void Move()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (grounded)
        {
            moving = moveDirection != new Vector3();
            if (moving)
            {
                AudioSource sonido = GetComponents<AudioSource>()[0];
                sonido.enabled = true;
            }
            else{
                AudioSource sonido = GetComponents<AudioSource>()[0];
                sonido.enabled = false;
            }
            rb.AddForce(moveDirection.normalized * moveSpeed, ForceMode.Force);
        }
        else
        {
            AudioSource sonido = GetComponents<AudioSource>()[0];
            sonido.enabled = false;
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
        AudioSource sonido = GetComponents<AudioSource>()[2];
        sonido.Play();
    }

    public void Slide(Vector3 moveDirection)
    {
        rb.AddForce(moveDirection.normalized * slideForce, ForceMode.Impulse);
    }

    private void GiroPositivoX()
    {
        if (rotationReady)
        {
            rotationReady = false;
            caida = inicio * Quaternion.Euler(new Vector3(90f, 0, 0));
            tiempo = 0.0f;
            AudioSource sonido = GetComponents<AudioSource>()[1];
            sonido.Play();
        }
    }

    private void GiroNegativoX()
    {
        if (rotationReady)
        {
            rotationReady = false;
            caida = inicio * Quaternion.Euler(new Vector3(-90f, 0, 0));
            tiempo = 0.0f;
            AudioSource sonido = GetComponents<AudioSource>()[1];
            sonido.Play();
        }
    }

    private void GiroPositivoZ()
    {
        if (rotationReady)
        {
            rotationReady = false;
            caida = inicio * Quaternion.Euler(new Vector3(0, 0, 90f));
            tiempo = 0.0f;
            AudioSource sonido = GetComponents<AudioSource>()[1];
            sonido.Play();
        }
    }
    private void GiroNegativoZ()
    {
        if (rotationReady)
        {
            rotationReady = false;
            caida = inicio * Quaternion.Euler(new Vector3(0, 0,-90f));
            tiempo = 0.0f;
            AudioSource sonido = GetComponents<AudioSource>()[1];
            sonido.Play();
        }
    }


    private void JumpReset()
    {
        jumpReady = true;
    }

    private void DoubleJumpReset()
    {
        doubleJumpReady = true;
    }

    private void RotationReset()
    {
        rotationReady = true;
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
