using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed; // Velocidad
    public float rotationSpeed;

    public float sprintMultiplier; // Multiplicador sprint
    private bool isSprinting; // Booleano que indica si esta esprintando

    [SerializeField]
    private bool canMove; // Booleano que indica si puede moverse o no

    // Inicializacion de variables
    void Start()
    {
        canMove = true;
        speed = 7f;
        rotationSpeed = 2f;
        sprintMultiplier = 2f;
        isSprinting = false;
    }

    void Update()
    {
        if(canMove)
        {
            // Movimiento WASD o flechas
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 moveDirection = Quaternion.Euler(0, transform.eulerAngles.y, 0) * new Vector3(x, 0, z);

            // Manejo de sprint
            isSprinting = Input.GetKey(KeyCode.LeftShift);

            float currentMoveSpeed = speed * (isSprinting ? sprintMultiplier : 1f);

            Vector3 newPosition = transform.position + moveDirection * currentMoveSpeed * Time.deltaTime;

            transform.position = newPosition;

            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            Vector3 rotation = new Vector3(-mouseY, mouseX, 0f) * rotationSpeed;
            transform.eulerAngles += rotation;
        }
        
    }

    // Setea el booleano canMove (usado en el menu de pausa)
    public void SetCanMove(bool move)
    {
        canMove = move;
    }
}
