using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    public int maxHealth = 100;
    public int currentHealth;
    public int attackValue = 5;

    public float attackRange = 5f;

    public HealthBar healthBar;

    public GameOverUIManager gameOverUIManager;

    public Terrain terrain;

    public LayerMask enemyLayer;

    private bool isGrounded = true;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 spawnPosition = transform.position;
        TerrainData terrainData = terrain.terrainData;
        float terrainHeight = terrain.SampleHeight(new Vector3(spawnPosition.x, 0, spawnPosition.z));
        if(spawnPosition.y < terrainHeight)
        {
            spawnPosition.y = terrainHeight + 1f;
            transform.position = spawnPosition;
        }
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            PerformMeleeAttack();
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public void TakeDamage(int damage)
    {
        // Reduce enemy's health by the damage amount.
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);

        // Check if the enemy's health reaches zero or below and handle death if needed.
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int healing)
    {
        currentHealth += healing;
        healthBar.SetHealth(currentHealth);
    }

    public void HealP(float p)
    {
        int healing = (int)(maxHealth * p);
        Heal(healing);
    }

    void Die()
    {
        // Implement death behavior here, e.g., play death animations, drop loot, etc.
        gameOverUIManager.ShowGameOverScreen();
        Destroy(gameObject);
    }

    void PerformMeleeAttack()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, attackRange))
        {
            // Check if the hit object is an enemy.
            if (hit.collider.CompareTag("Enemy"))
            {
                // Handle enemy hit. You can call a method on the enemy script to apply damage.
                EnemyScript enemyScript = hit.collider.GetComponent<EnemyScript>();
                enemyScript.TakeDamage(attackValue);
            }
        }

        /* Collider[] enemyCollider = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);
        if (enemyCollider.Length > 0)
        {
            foreach (Collider collider in enemyCollider)
            {
                EnemyScript enemyScript = collider.GetComponent<EnemyScript>();
                enemyScript.TakeDamage(attackValue);
                break;
            }
        } */
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * 8f, ForceMode.Impulse);
    }

}
