using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hipopotamo : EnemyScript
{
    Rigidbody cuerpo;
    public float impulseForce = 80;

    public LayerMask whatIsPlayer;
    public LayerMask whatIsEnemy;
    public hipoAttack ataqueDistancia;
    private Vector3 attackPoint;

    // Start is called before the first frame update
    void Start()
    {
        cuerpo = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform; // Encuentra al jugador por el tag
        //SetStats(maxHealth, attackValue, moveSpeed);
        SetStats(maxHealth, attackValue, moveSpeed, enemyLevel);
    }

    // Update is called once per frame
    void Update()
    {
        // Manejo enfriamiento del ataque
        timeSinceLastAttack += Time.deltaTime;

        bool playerInRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        bool playerInRangeOfMeleeAttack = Physics.CheckSphere(transform.position, attackRange * 0.75f, whatIsPlayer);
        bool playerInRangeOfRangedAttack = Physics.CheckSphere(transform.position, attackRange + 5, whatIsPlayer);
        bool enemyInRange = Physics.CheckSphere(transform.position, attackRange, whatIsEnemy);

        if (!playerInRange && enemyInRange && timeSinceLastAttack > attackCooldown) throwTeammate(); // lanza otro enemigo al jugador
        else if (!playerInRange && playerInRangeOfRangedAttack && timeSinceLastAttack > attackCooldown * 0.975f) RangedAttack();
        else if (playerInRangeOfMeleeAttack && timeSinceLastAttack >= attackCooldown *0.9f) MeleeAttack(); // Ataca
        else if (!playerInRange) chase(); // persigue al jugador
    }

    void chase()
    {
        Vector3 playerPosition = player.position;
        Vector3 moveDirection = (playerPosition - transform.position).normalized;
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

    }

    void throwTeammate()
    {
        GameObject teammate = FindClosest();

        teammate.transform.position = transform.position + new Vector3(0,4,0);
        teammate.GetComponent<Rigidbody>().AddForce((player.position - transform.position).normalized * impulseForce * 0.95f, ForceMode.Impulse);

        timeSinceLastAttack = 0f;
    }

    public GameObject FindClosest()
	{
		float distanceToClosestFruit = Mathf.Infinity;
		GameObject closestFruit = null;
		GameObject[] allFruit = GameObject.FindGameObjectsWithTag("Enemy");

		foreach (GameObject currentFruit in allFruit) {
			float distanceToFruit = (currentFruit.transform.position - this.transform.position).sqrMagnitude;
			if (distanceToFruit < distanceToClosestFruit) {
				distanceToClosestFruit = distanceToFruit;
				closestFruit = currentFruit;
			}
		}

        return closestFruit;

	}

    void MeleeAttack()
    {   
        cuerpo.AddForce((player.position - transform.position + new Vector3(0,2,0)).normalized * impulseForce, ForceMode.Impulse);
        timeSinceLastAttack = 0f;

    }
    void RangedAttack()
    {
        StartCoroutine(rangeProyectiles());
        timeSinceLastAttack = 0f;
    }

    IEnumerator rangeProyectiles()
    {
        Vector3 directionAttack = (player.position - transform.position).normalized;
        yield return new WaitForSeconds(0.4f);
        Instantiate(ataqueDistancia, (transform.position + directionAttack*4), Quaternion.identity);
        yield return new WaitForSeconds(0.4f);
        Instantiate(ataqueDistancia, (transform.position + directionAttack*8), Quaternion.identity);
        yield return new WaitForSeconds(0.4f);
        Instantiate(ataqueDistancia, (transform.position + directionAttack*12), Quaternion.identity);
    }

    void OnCollisionEnter(Collision other)
    {
        PlayerScript playerScript = player.GetComponent<PlayerScript>();
        if (other.collider.tag == "Player"){
            playerScript.TakeDamage(attackValue);
        }
    }



}
