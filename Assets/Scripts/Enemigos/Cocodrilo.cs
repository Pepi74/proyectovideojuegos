using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cocodrilo : EnemyScript
{
    Rigidbody cuerpo;
    public float impulseForce = 80;

    public LayerMask whatIsPlayer;

    private bool attackFinish = true;
    private Vector3 attackPoint;
    // Start is called before the first frame update
    void Start()
    {
        cuerpo = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform; // Encuentra al jugador por el tag		
        //SetStats(maxHealth, attackValue, moveSpeed, enemyLevel);
		attackCooldown = Random.Range(4.5f,5.5f);
		attackRange = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        // Manejo enfriamiento del ataque
        timeSinceLastAttack += Time.deltaTime;

        bool playerInRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInRange) chase();
        if (timeSinceLastAttack >= attackCooldown && (playerInRange && attackFinish)) attack(); // Ataca
    }

    void chase()
    {
        Vector3 playerPosition = player.position;
        Vector3 moveDirection = (playerPosition - transform.position).normalized;
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        attackFinish = true;

    }

    void attack()
    {   
        Debug.Log("Atacando");
        cuerpo.AddForce((player.position - transform.position + new Vector3(0,1,0)).normalized * impulseForce, ForceMode.Impulse);
        timeSinceLastAttack = 0f;
        attackFinish = false;

    }

    void OnCollisionEnter(Collision other)
    {
        PlayerScript playerScript = player.GetComponent<PlayerScript>();
        if (other.collider.tag == "Player"){
            playerScript.TakeDamage(attackValue);
            attackFinish = true;
        }
    }

}
