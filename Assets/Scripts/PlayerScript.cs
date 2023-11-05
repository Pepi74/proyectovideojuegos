using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    public int maxHealth = 100; // Vida maxima
    public int currentHealth; // Vida actual
    public int attackValue = 5; // Danio de ataque
    public float attackRange = 5f; // Rango de ataque
    public float jumpForce = 20f; // Fuerza del salto
    public HealthBar healthBar; // Barra de vida
    public GameOverUIManager gameOverUIManager; // UI game over
    public Terrain terrain; // Terreno
    public LayerMask enemyLayer; // Layer enemigo
    [SerializeField] private bool isGrounded = true; // Booleano que representa si esta en el piso
    private Rigidbody rb; // Rigidbody del jugador

    private bool fixSpawn = true;

    void Start()
    {
        // Manejo de spawn inicial al centro del terreno
        Vector3 spawnPosition;
        TerrainData terrainData = terrain.terrainData;
        float centerX = terrainData.size.x / 2f;
        float centerZ = terrainData.size.z / 2f;
        spawnPosition = new Vector3(centerX, 0, centerZ);
        float terrainHeight = terrain.SampleHeight(spawnPosition);
        if (spawnPosition.y < terrainHeight)
        {
            spawnPosition.y = terrainHeight + 1f;
        }
        transform.position = spawnPosition;
        // Inicializacion de vida
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        // Componente rigidbody
        rb = GetComponent<Rigidbody>();
        // Bloquear el cursor en el centro
        Cursor.lockState = CursorLockMode.Locked;
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
        //ataque a Melee
        if (Input.GetMouseButtonDown(1))
        {
            MeleeAttack();
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

    // Daño al jugador
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
        Cursor.lockState = CursorLockMode.None;
    }


    // Manejo de salto
    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
    // Checkear si esta en tierra
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 6) isGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 6) isGrounded = false;
    }
}
