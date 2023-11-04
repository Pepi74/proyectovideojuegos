using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed; // Adjust the move speed as needed.
    [SerializeField]
    private int maxHealth;
    [SerializeField]
    private int currentHealth;
    [SerializeField]
    private int attackValue;
    private Transform player; // Reference to the player for chasing.

    public float attackCooldown = 1.0f; // Set the cooldown duration in the Inspector.
    private float timeSinceLastAttack;

    public float attackRange;
    
    public HealthBar healthBar;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // Find the player by tag.
        attackRange = 3f;
    }

    void Update()
    {
        // Chase the player.
        if (player != null)
        {
            Vector3 playerPosition = player.position;
            Vector3 moveDirection = (playerPosition - transform.position).normalized;
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        }

        timeSinceLastAttack += Time.deltaTime;
        if (timeSinceLastAttack >= attackCooldown)
        {
            PerformEnemyMeleeAttack();
            timeSinceLastAttack = 0.0f; // Reset the timer.
        }

        Vector3 enemyPosition = transform.position;
        transform.position = checkHeight(enemyPosition);
        
    }

    // Method to customize the enemy's stats based on the D20 roll.
    public void SetStats(int health, int attack, float speed)
    {
        moveSpeed = speed;
        maxHealth = health;
        currentHealth = health;
        attackValue = attack;
        healthBar.SetMaxHealth(maxHealth);
    }

    // Method for taking damage.
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Method for enemy death.
    void Die()
    {
        // Implement death behavior here, e.g., play death animations, drop loot, etc.
        Destroy(gameObject);
    }

    void PerformEnemyMeleeAttack()
    {
        PlayerScript playerScript = player.GetComponent<PlayerScript>();
        // Check if the player is within the attack range, and apply damage if so.
        if (Vector3.Distance(transform.position, player.transform.position) < attackRange)
        {
            playerScript.TakeDamage(attackValue); // Assuming you have a TakeDamage method in the player's script.
        }
    }

    Vector3 checkHeight(Vector3 enemyPosition)
    {
        int terrainLayerMask = 1 << LayerMask.NameToLayer("Terrain");
        RaycastHit hit;
        if(Physics.Raycast(enemyPosition, new Vector3(0,-1,0), out hit, Mathf.Infinity, terrainLayerMask))
        {
            float terrainHeight = hit.point.y;
            if (enemyPosition.y - terrainHeight <= 2) enemyPosition.y = terrainHeight + 2f;            
        }

        return enemyPosition;
    }
}   
