using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerScript : MonoBehaviour
{

    public int maxHealth = 100; // Vida maxima
    public int currentHealth; // Vida actual
    public float maxStamina = 100;
    public float currentStamina;
    public int attackValue = 5; // Danio de ataque
    public float attackRange = 10f; // Rango de ataque
    public HealthBar healthBar; // Barra de vida
    public StaminaBar staminaBar;
    private int staminaRegenRate = 10;
    public GameOverUIManager gameOverUIManager; // UI game over
    public LayerMask enemyLayer; // Layer enemigo
    [SerializeField]
    private bool isTired = false;
    [SerializeField]
    private bool canRegen = true;
    private Rigidbody rb; // Rigidbody del jugador
    [SerializeField]
    private int playerLevel = 1;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI levelUpText;

    void Start()
    {
        gameOverUIManager = GameObject.Find("GameOverUI").GetComponent<GameOverUIManager>();
        // Inicializacion de vida
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        currentStamina = maxStamina;
        staminaBar.SetMaxStamina(maxStamina);
        // Componente rigidbody
        rb = GetComponent<Rigidbody>();
        levelUpText = transform.Find("PlayerUI").Find("Level").Find("LevelUpText").GetComponent<TextMeshProUGUI>();
        levelUpText.text = "";
        levelText = transform.Find("PlayerUI").Find("Level").Find("LevelText").GetComponent<TextMeshProUGUI>();
        levelText.text = "Level: " + playerLevel.ToString();
    }

    void Update()
    {
        // Ataque si es que se presiona el click izquierdo del mouse
        if (Input.GetMouseButtonDown(0) && canRegen)
        {
            Attack();
            StaminaChange(-10);
        }

        if (Input.GetMouseButtonDown(0))
        {
            MeleeAttack();
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

        if (currentStamina < maxStamina && canRegen) StaminaRegen(staminaRegenRate);

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

    // Recuperar un porcentaje de vida y stamina
    public void HealP(float p)
    {
        int healing = (int)(maxHealth * p);
        Heal(healing);
        StaminaChange(maxStamina * (p/2));
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
                // Si la distancia es menor o igual al rango de ataque, el enemigo es dañado
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

    IEnumerator tired(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        StaminaChange(maxStamina / 4);
        canRegen = true;
    }

    public void SetCanRegen(bool value)
    {
        canRegen = value;
    }

    public void LevelUp()
    {
        playerLevel++;
        attackValue++;
        maxHealth += 20;
        maxStamina += 10;
        staminaRegenRate += 2;
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        currentStamina = maxStamina;
        staminaBar.SetMaxStamina(maxStamina);
        levelText.text = "Level: " + playerLevel.ToString();
        StartCoroutine(LevelUpTextDisplay());
    }

    IEnumerator LevelUpTextDisplay()
    {
        float endTime = Time.time + 2f;
        while(Time.time < endTime)
        {
            levelUpText.text = "<color=orange>Level Up!!</color>";
            yield return new WaitForSeconds(0.1f);
            levelUpText.text = "<color=yellow>Level Up!!</color>";
            yield return new WaitForSeconds(0.1f);
        }
        levelUpText.text = "";
    }
}
