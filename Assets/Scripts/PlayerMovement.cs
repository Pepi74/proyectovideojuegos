using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float rotationSpeed;

    public float sprintMultiplier; // Speed multiplier when sprinting
    private bool isSprinting;

    [SerializeField]
    private bool canMove;

    void Start()
    {
        canMove = true;
        speed = 7f;
        rotationSpeed = 2f;
        sprintMultiplier = 2f;
        isSprinting = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(canMove)
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            // Calculate the player's movement direction based on their rotation
            Vector3 moveDirection = Quaternion.Euler(0, transform.eulerAngles.y, 0) * new Vector3(x, 0, z);

            // Check if the Shift key is held down to sprint
            isSprinting = Input.GetKey(KeyCode.LeftShift);

            // Adjust the speed based on sprinting
            float currentMoveSpeed = speed * (isSprinting ? sprintMultiplier : 1f);

            // Calculate the new position
            Vector3 newPosition = transform.position + moveDirection * currentMoveSpeed * Time.deltaTime;

            // Apply the clamped position
            transform.position = newPosition;

            //float mouseX = Input.GetAxis("Mouse X");
            //float mouseY = Input.GetAxis("Mouse Y");

            //Vector3 rotation = new Vector3(0f, mouseX, 0f) * rotationSpeed;
            //transform.eulerAngles += rotation;
        }
        
    }

    public void SetCanMove(bool move)
    {
        canMove = move;
    }
}
