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
	public bool isGrabbed = false;

    public float attackRange; // Rango de ataque
    
    public HealthBar healthBar; // Barra de vida
    public int enemyLevel;

    public TextMeshProUGUI levelText;

    void Awake()
    {
        levelText = transform.Find("EnemyUI").Find("Level").Find("LevelText").GetComponent<TextMeshProUGUI>();
		player = GameObject.FindGameObjectWithTag("Player").transform; // Encuentra al jugador por el tag
    }

    void Update()
    {
        // Persigue al jugador
        if (player != null)
        {
            Vector3 playerPosition = player.position;
            Vector3 moveDirection = (playerPosition - transform.position).normalized;
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        }

        // Manejo enfriamiento del ataque
        timeSinceLastAttack += Time.deltaTime;
        if (timeSinceLastAttack >= attackCooldown)
        {
            //PerformEnemyMeleeAttack(); // Ataca
			//Debug.Log("Ataca");
            timeSinceLastAttack = 0.0f; // Resetea el timer
        }

        // Ajusta su valor Y para estar arriba del terreno (como flotando)
        Vector3 enemyPosition = transform.position;
        transform.position = checkHeight(enemyPosition);
        
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
    void Die()
    {
        Destroy(gameObject);
    }

    // TODO: Mejorar esto!
    // Manejo ataque del enemigo
    void PerformEnemyMeleeAttack()
    {
        PlayerScript playerScript = player.GetComponent<PlayerScript>();
        // Verifica si el jugador esta dentro de su rango de ataque, de ser asi lo ataca
        if (Vector3.Distance(transform.position, player.transform.position) <= attackRange)
        {
            playerScript.TakeDamage(attackValue); // El jugador es dañado
        }
    }

    // Manejo de la componente Y de la posicion del enemigo
    Vector3 checkHeight(Vector3 enemyPosition)
    {
        int terrainLayerMask = 1 << LayerMask.NameToLayer("Terrain");
        RaycastHit hit;
        if(Physics.Raycast(enemyPosition, Vector3.down, out hit, Mathf.Infinity, terrainLayerMask))
        {
            float terrainHeight = hit.point.y;
            if (enemyPosition.y - terrainHeight <= 2) enemyPosition.y = terrainHeight + 2f;            
        }

        return enemyPosition;
    }

	public void grabbed()
    {
        isGrabbed = true;
    }
}   
