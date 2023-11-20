using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed; // Velocidad

    public float sprintMultiplier; // Multiplicador sprint
    private bool isSprinting; // Booleano que indica si esta esprintando
    public Animator animator; //controlador de el animador
    [SerializeField] private bool canMove; // Booleano que indica si puede moverse o no

    // Inicializacion de variables
    void Start()
    {
        canMove = true;
        speed = 7f;
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

            animator.SetFloat("z speed", z);
            animator.SetFloat("x_speed", x);

            // Manejo de sprint
            isSprinting = Input.GetKey(KeyCode.LeftShift);

            float currentMoveSpeed = speed * (isSprinting ? sprintMultiplier : 1f);

            Vector3 newPosition = transform.position + moveDirection * currentMoveSpeed * Time.deltaTime;

            transform.position = newPosition;
        }
        
    }

    // Setea el booleano canMove (usado en el menu de pausa)
    public void SetCanMove(bool move)
    {
        canMove = move;
    }
}
