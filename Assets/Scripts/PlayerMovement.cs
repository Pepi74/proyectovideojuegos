using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed; // Velocidad

    public float sprintMultiplier; // Multiplicador sprint
    //private bool isSprinting; // Booleano que indica si esta esprintando

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

    public KeyCode jumpKey = KeyCode.Space;

    // Inicializacion de variables
    void Start()
    {
        canMove = true;
        moveSpeed = 7f;
        sprintMultiplier = 2f;
        //isSprinting = false;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        jumpForce = 10f;
        jumpCooldown = 0.25f;
        airMultiplier = 0.4f;
        readyToJump = true;
    }

    void Update()
    {
        if(canMove)
        {
            grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundLayerMask);

            // Movimiento WASD o flechas
            MyInput();
            SpeedControl();

            if (grounded)
                rb.drag = groundDrag;
            else
                rb.drag = 0;

            // Vector3 direction = new Vector3(x, 0f, z).normalized;

            // Vector3 moveDirection = Quaternion.Euler(0, transform.eulerAngles.y, 0) * new Vector3(x, 0, z);

            // Manejo de sprint
            // isSprinting = Input.GetKey(KeyCode.LeftShift);

            // float currentMoveSpeed = speed * (isSprinting ? sprintMultiplier : 1f);

            // Vector3 newPosition = transform.position + moveDirection * currentMoveSpeed * Time.deltaTime;

            // transform.position = newPosition;

            // float mouseX = Input.GetAxis("Mouse X");
            // float mouseY = Input.GetAxis("Mouse Y");

            // Vector3 rotation = new Vector3(-mouseY, mouseX, 0f) * rotationSpeed;
            // transform.eulerAngles += rotation;
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

        if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        else if(!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }
}
