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
}   
