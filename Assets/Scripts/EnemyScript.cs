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
	public void Grabbed()
    {
        isGrabbed = true;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    protected void CheckDistance()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        const float minDistance = 2f;

        foreach (GameObject enemy in enemies)
        {
            if (enemy == gameObject) continue;
            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            if (!(distance < minDistance)) continue;
            Vector3 direction = transform.position - enemy.transform.position;
            transform.Translate(direction.normalized * ((minDistance - distance) * Time.deltaTime), Space.World);
        }
    }

    protected void CheckYValue()
    {
        if (transform.position.y < -100)
        {
            Die();
        }
    }
}   
