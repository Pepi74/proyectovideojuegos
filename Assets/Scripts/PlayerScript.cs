using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    public int maxHealth = 100; // Vida maxima
    public int currentHealth; // Vida actual
    public float maxStamina = 100;
    public float currentStamina;
    public int attackValue = 5; // Danio de ataque
    public float attackRange = 5f; // Rango de ataque
    public float jumpForce = 20f; // Fuerza del salto
    public HealthBar healthBar; // Barra de vida
    public StaminaBar staminaBar;
    public GameOverUIManager gameOverUIManager; // UI game over
    public Terrain terrain; // Terreno
    public LayerMask enemyLayer; // Layer enemigo
    private bool isGrounded = true; // Booleano que representa si esta en el piso
    [SerializeField]
    private bool isTired = false;
    [SerializeField]
    private bool canRegen = true;
    private Rigidbody rb; // Rigidbody del jugador

    private bool fixSpawn = true;

    void Start()
    {
        // Inicializacion de vida
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        currentStamina = maxStamina;
        staminaBar.SetMaxStamina(maxStamina);
        // Componente rigidbody
        rb = GetComponent<Rigidbody>();
        // Bloquear el cursor en el centro
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Manejo de spawn inicial al centro del terreno
        // TODO: cambiar a que el player no este en la jerarquia y que spawnee cuando inicie el juego,
        // deberia arreglar este problema y dejarlo en la funcion Start.
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
        if (Input.GetMouseButtonDown(0) && canRegen)
        {
            Attack();
            StaminaChange(-20);
        }

        if (Input.GetMouseButtonDown(0))
        {
            MeleeAttack();
        }

        // Salto
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        // Manejo de vida si la vida actual excede la maxima
        if (currentHealth > maxHealth) currentHealth = maxHealth;

        if (isTired)
        {
            isTired = false;
            canRegen = false;
            StartCoroutine(tired(3f));
        }

        if (currentStamina <= 0)
        {
            currentStamina = 0;
            isTired = true;
        }

        if (currentStamina < maxStamina && canRegen) StaminaRegen(5f);

        if (currentStamina >= maxStamina) currentStamina = maxStamina;            
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

    public void StaminaChange(float value)
    {
        currentStamina += value;
        staminaBar.SetStamina(currentStamina);
    }

    public void StaminaRegen(float regenRate)
    {
        currentStamina += regenRate * Time.deltaTime;
        staminaBar.SetStamina(currentStamina);
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

    // Manejo de ataque
    void Attack()
    {
        // Raycast desde el centro de la camara hacia donde se apunta.
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);
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

    void MeleeAttack()
    {

    }

    // Manejo de salto
    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
    // Checkear si esta en tierra
    private void OnCollisionEnter()
    {
        isGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }

    public void SetCanRegen(bool value)
    {
        canRegen = value;
    }

    IEnumerator tired(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        StaminaChange(maxStamina/2);
        canRegen = true;
    }
}
