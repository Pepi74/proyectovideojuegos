using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    public int maxHealth = 100; // Vida maxima
    public int currentHealth; // Vida actual
    public int attackValue = 5; // Danio de ataque

    public float attackRange = 5f; // Rango de ataque

    public HealthBar healthBar; // Barra de vida

    public GameOverUIManager gameOverUIManager; // UI game over

    public Terrain terrain; // Terreno

    public LayerMask enemyLayer; // Layer enemigo

    private bool isGrounded = true; // Booleano que representa si esta en el piso (aun no lo implemento al script)

    private Rigidbody rb; // Rigidbody del jugador

    private bool fixSpawn = true;

    void Start()
    {
        // Inicializacion de vida
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        // Componente rigidbody
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Manejo de spawn inicial al centro del terreno
        if (fixSpawn)
        {
            TerrainData terrainData = terrain.terrainData;
            float centerX = terrainData.size.x / 2f;
            float centerZ = terrainData.size.z / 2f;
            float terrainHeight = terrain.SampleHeight(new Vector3(centerX, 0, centerZ));
            Vector3 spawnPosition = new Vector3(centerX, terrainHeight + 1f, centerZ);
            transform.position = spawnPosition;
            fixSpawn = false;
        }
        // Ataque si es que se presiona el click izquierdo del mouse
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }

        // Salto
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        // Manejo de vida si la vida actual excede la maxima
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    // Danio al jugador
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        // Si la vida es menor o igual a 0, muere
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Recuperar vida
    public void Heal(int healing)
    {
        currentHealth += healing;
        healthBar.SetHealth(currentHealth);
    }

    // Recuperar un porcentaje de vida
    public void HealP(float p)
    {
        int healing = (int)(maxHealth * p);
        Heal(healing);
    }

    // Manejo de muerte del jugador
    void Die()
    {
        gameOverUIManager.ShowGameOverScreen();
        Destroy(gameObject);
    }

    // Manejo de ataque
    void Attack()
    {
        // Raycast desde el centro de la camara con direccion al cursor
        Vector3 mousePosition = Input.mousePosition;

        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f))
        {
            // Raycast colisiona con enemigo
            if (hit.collider.CompareTag("Enemy"))
            {
                // Calcula la distancia entre el jugador y el enemigo
                float distance = Vector3.Distance(transform.position, hit.collider.transform.position);
                //Debug.Log(distance);
                // Si la distancia es menor o igual al rango de ataque, el enemigo es daniado
                if (distance <= attackRange)
                {
                    EnemyScript enemyScript = hit.collider.GetComponent<EnemyScript>();
                    enemyScript.TakeDamage(attackValue);
                }
            }
        }
    }

    // Manejo de salto
    void Jump()
    {
        rb.AddForce(Vector3.up * 20f, ForceMode.Impulse);
    }

}
