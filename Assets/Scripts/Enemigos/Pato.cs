using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pato : EnemyScript
{
    Rigidbody cuerpo;

    public float rangeStomp = 2;

    public LayerMask whatIsPlayer;

    private bool attackFinish = true;
    private bool attackPointSet = false;
    private Vector3 attackPoint;
    // Start is called before the first frame update
    void Start()
    {
        cuerpo = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform; // Encuentra al jugador por el tag
        //SetStats(maxHealth, attackValue, moveSpeed, enemyLevel);
    }

    // Update is called once per frame
    void Update()
    {
        // Manejo enfriamiento del ataque
        timeSinceLastAttack += Time.deltaTime;

        bool playerInRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInRange) chase();
        if (timeSinceLastAttack >= attackCooldown && (playerInRange || attackFinish)) attack(); // Ataca
    }

    void chase()
    {
        Vector3 playerPosition = player.position;
        Vector3 moveDirection = (playerPosition - transform.position).normalized;
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

    }

    void attack()
    {   
        if (!attackPointSet) {
            attackPoint = player.position + new Vector3(0,10,0);
            attackPointSet = true;
        }
        
        if (attackPointSet) {
            cuerpo.useGravity = false; cuerpo.isKinematic = true;
            transform.position = Vector3.MoveTowards(transform.position,attackPoint,10 * Time.deltaTime);
        }

        Vector3 distanceToPlayer = transform.position - attackPoint;

        if (distanceToPlayer.magnitude < 1f){
            cuerpo.useGravity = true; cuerpo.isKinematic = false;
            cuerpo.AddForce(-transform.up * 50);
            attackPointSet = false;
            attackFinish = false;
            
            timeSinceLastAttack = 0.0f; // Resetea el timer

        }

    }

    void OnCollisionEnter(Collision other)
    {
        PlayerScript playerScript = player.GetComponent<PlayerScript>();
        if (other.collider.tag == "Player"){
            playerScript.TakeDamage((int)(attackValue * 1.2f));
            attackFinish = true;

        } else if (other.collider.tag == "Terrain" && !attackFinish) {
            bool playerInRange = Physics.CheckSphere(transform.position, rangeStomp, whatIsPlayer);
            playerScript.TakeDamage(attackValue);
            attackFinish = true;
        }
    }

}
