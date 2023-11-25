using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zorrillo : EnemyScript
{
    public int cantidadEmitters;
    public float aguante = 20;
    public LayerMask whatIsPlayer;
    public Animator animator;
    private Transform controllerUp;
    private Transform controllerMid;
    private Transform controllerDown;
    private bool attackStarted = false;
    // Start is called before the first frame update
    void Start()
    {
        SetStats(maxHealth, attackValue, moveSpeed, enemyLevel);
        controllerMid = this.gameObject.transform.GetChild(1);
        controllerUp = this.gameObject.transform.GetChild(2);
        controllerDown = this.gameObject.transform.GetChild(3);
    }

    // Update is called once per frame
    void Update()
    {
        bool playerInRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        timeSinceLastAttack += Time.deltaTime;

        if (attackStarted) {
            if (timeSinceLastAttack >= aguante){
                chase();
            } 
        } else {
            if (!playerInRange) chase();
            if (playerInRange) attack();

            timeSinceLastAttack = 0;

        }

        


    }
    // Emitter amount cantidad de emisores
    // 

    void chase() 
    {
        Vector3 playerPosition = player.position;
        Vector3 moveDirection = (playerPosition - transform.position).normalized;
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        animator.SetBool("atk", false);
    }

    void attack() 
    {
        
        BS_Controller up = controllerUp.GetComponent<BS_Controller>();
        BS_Controller middle = controllerMid.GetComponent<BS_Controller>();
        BS_Controller down = controllerDown.GetComponent<BS_Controller>();

        up.emitterAmount = cantidadEmitters;
        middle.emitterAmount = cantidadEmitters;
        down.emitterAmount = cantidadEmitters;
        animator.SetBool("atk", true);
        attackStarted = true;

    }

}