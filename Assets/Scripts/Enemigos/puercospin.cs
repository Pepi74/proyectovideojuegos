using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class puercospin : EnemyScript
{
    public int cantidadEspinas;
    public LayerMask whatIsPlayer;
    private Transform controllerUp;
    private Transform controllerMid;
    private Transform controllerDown;
    // Start is called before the first frame update
    void Start()
    {
        //SetStats(maxHealth, attackValue, moveSpeed, enemyLevel);
        controllerMid = this.gameObject.transform.GetChild(1);
        controllerUp = this.gameObject.transform.GetChild(2);
        controllerDown = this.gameObject.transform.GetChild(3);
		attackCooldown = Random.Range(4.5f, 5.5f);
		attackRange = 6f;
    }

    // Update is called once per frame
    void Update()
    {
        bool playerInRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInRange) chase();
        if (playerInRange) attack();


    }
    // Emitter amount cantidad de emisores
    // 

    void chase() 
    {
        Vector3 playerPosition = player.position;
        Vector3 moveDirection = (playerPosition - transform.position).normalized;
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);        
    }

    void attack() 
    {
        BS_Controller up = controllerUp.GetComponent<BS_Controller>();
        BS_Controller middle = controllerMid.GetComponent<BS_Controller>();
        BS_Controller down = controllerDown.GetComponent<BS_Controller>();

        up.emitterAmount = cantidadEspinas;
        middle.emitterAmount = cantidadEspinas;
        down.emitterAmount = cantidadEspinas;

    }
}
