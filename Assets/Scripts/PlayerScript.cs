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
        Vector3 spawnPosition;
        TerrainData terrainData = terrain.terrainData;
        float centerX = terrainData.size.x / 2f;
        float centerZ = terrainData.size.z /2f;
        spawnPosition = new Vector3(centerX, 0, centerZ);
        float terrainHeight = terrain.SampleHeight(spawnPosition);
        if(spawnPosition.y < terrainHeight)
        {
            spawnPosition.y = terrainHeight + 1f;
        }
        transform.position = spawnPosition;
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
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

    void Attack()
    {
        Vector3 mousePosition = Input.mousePosition;

        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f))
        {
            // Check if the hit object is an enemy.
            if (hit.collider.CompareTag("Enemy"))
            {
                // Handle enemy hit. You can call a method on the enemy script to apply damage.
                float distance = Vector3.Distance(transform.position, hit.collider.transform.position);
                Debug.Log(distance);
                if (distance <= attackRange)
                {
                    EnemyScript enemyScript = hit.collider.GetComponent<EnemyScript>();
                    enemyScript.TakeDamage(attackValue);
                }
            }
        }
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * 20f, ForceMode.Impulse);
    }

}
