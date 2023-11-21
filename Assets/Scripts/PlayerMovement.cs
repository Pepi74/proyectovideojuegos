using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float moveSpeed; // Velocidad
    public float walkSpeed;
    public float sprintSpeed;

    [SerializeField]
    private bool canMove; // Booleano que indica si puede moverse o no

    public Transform orientation;

    private float horizontalInput;
    private float verticalInput;

    private Vector3 moveDirection;

    private Rigidbody rb;

    public float groundDrag;
    public float playerHeight;
    public LayerMask groundLayerMask;
    private bool grounded;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    private bool readyToJump;

    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;

    public MovementState state;

    public enum MovementState
    {
        Walking,
        Sprinting,
        Air
    }

    public PlayerScript playerScript;

    public Animator animator;

    // Inicializacion de variables
    void Start()
    {
        canMove = true;
        walkSpeed = 5f;
        sprintSpeed = 7f;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        playerHeight = transform.localScale.y * 2;
        jumpForce = 10f;
        jumpCooldown = 0.25f;
        airMultiplier = 0.4f;
        readyToJump = true;
        maxSlopeAngle = 40f;
        playerScript = GetComponent<PlayerScript>();
    }

    void Update()
    {

        if(canMove)
        {
            grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundLayerMask);
            animator.SetBool("grounded", grounded);

            // Movimiento WASD o flechas
            MyInput();
            SpeedControl();
            StateHandler();

            if (grounded)
                rb.drag = groundDrag;
            else
                rb.drag = 0;
        }        
        
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    // Setea el booleano canMove (usado en el menu de pausa)
    public void SetCanMove(bool move)
    {
        canMove = move;
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        
        animator.SetFloat("z speed", verticalInput);
        animator.SetFloat("x_speed", horizontalInput);

        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if(OnSlope())
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down * 40f, ForceMode.Force);
            }
        }

        else if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        else if(!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

        rb.useGravity = !OnSlope();
    }

    private void SpeedControl()
    {

        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > moveSpeed)
                rb.velocity = rb.velocity.normalized * moveSpeed;
        }

        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }        
    }

    private void Jump()
    {
        exitingSlope = true;

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;

        exitingSlope = false;
    }

    private void StateHandler()
    {
        if (grounded && Input.GetKey(sprintKey) && (int) playerScript.currentStamina != 0)
        {
            state = MovementState.Sprinting;
            moveSpeed = sprintSpeed;
        }

        else if (grounded)
        {
            state = MovementState.Walking;
            moveSpeed = walkSpeed;
        }

        else
        {
            state = MovementState.Air;
            animator.SetFloat("y_speed", rb.velocity.y);
        }
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }
}
