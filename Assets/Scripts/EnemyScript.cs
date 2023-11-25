using UnityEngine;
using TMPro;

public class EnemyScript : MonoBehaviour
{
    public float moveSpeed; // Velocidad de movimiento
    public int maxHealth; // Vida maxima
    [SerializeField]
    private int currentHealth; // Vida actual
    public int attackValue; // Daño de ataque
    public Transform player; // Posicion del jugador

    public float attackCooldown; // Enfriamiento de ataque (implementar mejor esta mecanica)
    public float timeSinceLastAttack; // Tiempo desde el ultimo ataque
	public bool isGrabbed;

    public float attackRange; // Rango de ataque
    
    public HealthBar healthBar; // Barra de vida
    public int enemyLevel;

    public TextMeshProUGUI levelText;

    private void Awake()
    {
        levelText = transform.Find("EnemyUI").Find("Level").Find("LevelText").GetComponent<TextMeshProUGUI>();
		player = GameObject.FindGameObjectWithTag("Player").transform; // Encuentra al jugador por el tag
    }

    private void Update()
    {
        // Persigue al jugador
        if (player != null)
        {
            Vector3 playerPosition = player.position;
            Vector3 moveDirection = (playerPosition - transform.position).normalized;
            transform.Translate(moveDirection * (moveSpeed * Time.deltaTime));
        }

        // Manejo enfriamiento del ataque
        timeSinceLastAttack += Time.deltaTime;
        if (timeSinceLastAttack >= attackCooldown)
        {
            //PerformEnemyMeleeAttack(); // Ataca
			//Debug.Log("Ataca");
            timeSinceLastAttack = 0.0f; // Resetea el timer
        }
        
    }

    // Inicializacion de variables (desde las variables del resultado del d20)
    public void SetStats(int health, int attack, float speed, int level)
    {
        moveSpeed = speed;
        maxHealth += health;
        currentHealth = maxHealth;
        attackValue += attack;
        healthBar.SetMaxHealth(maxHealth);
        enemyLevel = level;
        levelText.text = "Lv: " + enemyLevel.ToString();
    }

    // Daño al enemigo
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Muerte
    private void Die()
    {
        Destroy(gameObject);
    }

    // TODO: Mejorar esto!
    // Manejo ataque del enemigo
/*
    void PerformEnemyMeleeAttack()
    {
        PlayerScript playerScript = player.GetComponent<PlayerScript>();
        // Verifica si el jugador esta dentro de su rango de ataque, de ser asi lo ataca
        if (Vector3.Distance(transform.position, player.transform.position) <= attackRange)
        {
            playerScript.TakeDamage(attackValue); // El jugador es dañado
        }
    }
*/

	public void Grabbed()
    {
        isGrabbed = true;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    protected void CheckDistance()
    {
        // Find all enemies in the scene
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        const float minDistance = 5f;

        foreach (GameObject enemy in enemies)
        {
            // Check if the enemy is not the current one
            if (enemy == gameObject) continue;
            // Calculate the distance between the current enemy and the other enemy
            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            // If the distance is less than the minimum distance, adjust the position
            if (!(distance < minDistance)) continue;
            Vector3 direction = transform.position - enemy.transform.position;
            transform.Translate(direction.normalized * ((minDistance - distance) * Time.deltaTime), Space.World);
        }
    }
}   
