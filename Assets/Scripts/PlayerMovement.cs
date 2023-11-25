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
    
    private static readonly int Grounded = Animator.StringToHash("grounded");
    private static readonly int ZSpeed = Animator.StringToHash("z_speed");
    private static readonly int XSpeed = Animator.StringToHash("x_speed");
    private static readonly int YSpeed = Animator.StringToHash("y_speed");

    // Inicializacion de variables
    private void Start()
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

    private void Update()
    {
        if (!canMove) return;
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundLayerMask);
        animator.SetBool(Grounded, grounded);

        // Movimiento WASD o flechas
        MyInput();
        SpeedControl();
        StateHandler();

        if (grounded) rb.drag = groundDrag;
        else rb.drag = 0;
    }

    private void FixedUpdate()
    {
        if (!canMove)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        else
        {
            MovePlayer();
        }
    }

    // Setea el booleano canMove
    public void SetCanMove(bool move)
    {
        canMove = move;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        
        animator.SetFloat(ZSpeed, verticalInput);
        animator.SetFloat(XSpeed, horizontalInput);

        if (!Input.GetKey(jumpKey) || !readyToJump || !grounded) return;
        readyToJump = false;

        Jump();

        Invoke(nameof(ResetJump), jumpCooldown);
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if(OnSlope())
        {
            rb.AddForce(GetSlopeMoveDirection() * (moveSpeed * 20f), ForceMode.Force);

            if (rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down * 40f, ForceMode.Force);
            }
        }

        else switch (grounded)
        {
            case true:
                rb.AddForce(moveDirection.normalized * (moveSpeed * 10f), ForceMode.Force);
                break;
            case false:
                rb.AddForce(moveDirection.normalized * (moveSpeed * 10f * airMultiplier), ForceMode.Force);
                break;
        }

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
            var velocity = rb.velocity;
            Vector3 flatVel = new Vector3(velocity.x, 0f, velocity.z);

            if (!(flatVel.magnitude > moveSpeed)) return;
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }        
    }

    private void Jump()
    {
        exitingSlope = true;

        var velocity = rb.velocity;
        velocity = new Vector3(velocity.x, 0f, velocity.z);
        rb.velocity = velocity;

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;

        exitingSlope = false;
    }

    private void StateHandler()
    {
        switch (grounded)
        {
            case true when Input.GetKey(sprintKey) && (int) playerScript.currentStamina != 0:
                state = MovementState.Sprinting;
                moveSpeed = sprintSpeed;
                break;
            case true:
                state = MovementState.Walking;
                moveSpeed = walkSpeed;
                break;
            default:
                state = MovementState.Air;
                animator.SetFloat(YSpeed, rb.velocity.y);
                break;
        }
    }

    private bool OnSlope()
    {
        if (!Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f)) return false;
        float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
        return angle < maxSlopeAngle && angle != 0;

    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }
}
