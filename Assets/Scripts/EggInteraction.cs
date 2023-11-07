using System.Collections;
using UnityEngine;
using TMPro;

// TODO: mejorar este script quizas?
public class EggInteraction : MonoBehaviour
{
    public float interactionRange = 2.0f; // Rango de interaccion de huevos
    public TextMeshProUGUI interactionText; // Texto de ui interaccion
    public TextMeshProUGUI resultText; // Texto de resultado d20
    public TextMeshProUGUI rollStartText; // Texto "Rolling D20...", iniciado en HandleInteraction
    public GameObject enemyPrefab; // Prefab del enemigo
    public GameObject currentEgg; // Huevo que se interactua
    public PlayerScript playerScript; // Componente PlayerScript del jugador
    [SerializeField]
    private bool interacting = false; // Booleano que indica si se esta interactuando o no

    public LayerMask eggLayer; // Layer del huevo

    void Start()
    {
        interactionText = GameObject.Find("InteractText").GetComponent<TextMeshProUGUI>();
        interactionText.text = "Press <color=yellow>E</color> to interact";
        interactionText.gameObject.SetActive(false);
        rollStartText = GameObject.Find("RollStartText").GetComponent<TextMeshProUGUI>();
        rollStartText.text = "Rolling D20...";
        rollStartText.gameObject.SetActive(false);
        resultText = GameObject.Find("ResultText").GetComponent<TextMeshProUGUI>();
        resultText.gameObject.SetActive(false);
    }

    void Update()
    {
        //Spherecast que detecta huevos cercanos al jugador
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, interactionRange, eggLayer);

        if (hitColliders.Length > 0)
        {

            foreach (Collider collider in hitColliders)
            {
                if (collider.CompareTag("Egg") && !interacting)
                {
                    ShowInteractionText();
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        // Cuando se presiona "E", se maneja la interaccion
                        interacting = true;
                        HandleInteraction(collider.gameObject);
                    }
                }

                else
                {
                    HideInteractionText();
                }
            }
        
        }

        else
        {
            HideInteractionText();
        }
        
    }

    void ShowInteractionText()
    {
        // Muestra texto interaccion
        interactionText.gameObject.SetActive(true);
    }

    void HideInteractionText()
    {
        // Oculta texto interaccion
        interactionText.gameObject.SetActive(false);
    }

    void HandleInteraction(GameObject egg)
    {
        // Manejo de interaccion

        currentEgg = egg; // Asigna el huevo interactuado a la variable

        rollStartText.gameObject.SetActive(true); // Muestra el texto "Rolling D20..."

        // Corutina del manejo del d20
        StartCoroutine(DisplayRandomNumbersAndResult(egg));
    }

    // Numero entero random entre 1 y 20
    int RollD20()
    {
        return Random.Range(1, 21);
    }

    // Manejo del d20 y resultados dependiendo del roll del d20
    IEnumerator DisplayRandomNumbersAndResult(GameObject egg)
    {
        int randomNumbersDuration = 2; // Duracion de numeros random antes del resultado final
        float endTime = Time.time + randomNumbersDuration;
        TextMeshProUGUI textMeshProText = resultText;
        TextMeshProUGUI rollStartTextMeshPro = rollStartText;

        textMeshProText.gameObject.SetActive(true);
        // Muestra numeros random antes del resultado final del d20.
        while (Time.time < endTime)
        {
            int randomValue = Random.Range(1, 21);
            textMeshProText.text = randomValue.ToString();
            yield return new WaitForSeconds(0.1f);
        }

        // Resultado final.
        int finalResult = RollD20();
        string finalResultString = finalResult.ToString();
        // Iniciacion de variables que cambiaran dependiendo del resultado final del d20
        string colorPrefix = "";
        string colorSuffix = "";
        int numberOfEnemies = 0;
        int attackValue = 0;
        int health = 0;
        bool healing = false;
        float healingValue = 0f;
        float speed = 0f;
        // Manejo del roll igual a 1
        if (finalResult == 1)
        {
            colorPrefix = "<color=red>";
            colorSuffix = "</color>";
            numberOfEnemies = 12;
            attackValue = 5;
            health = 20;
            speed = 10f;
        }
        // Manejo del roll entre 2 y 7
        if (finalResult > 1 && finalResult <= 7)
        {
            colorPrefix = "<color=orange>";
            colorSuffix = "</color>";
            numberOfEnemies = Random.Range(9, 12 - finalResult / 2);
            attackValue = 4;
            health = 20 - finalResult / 2;
            speed = 8f;
        }
        // Manejo del roll entre 8 y 13
        if (finalResult > 7 && finalResult <= 13)
        {
            colorPrefix = "<color=yellow>";
            colorSuffix = "</color>";
            numberOfEnemies = Random.Range(6, 9 - finalResult / 4);
            attackValue = 3;
            health = 15 - finalResult / 3;
            speed = 6f;
        }
        // Manejo del roll entre 14 y 19
        if (finalResult > 13 && finalResult <= 19)
        {
            colorPrefix = "<color=blue>";
            colorSuffix = "</color>";
            numberOfEnemies = Random.Range(2, 6 - finalResult / 8);
            attackValue = 2;
            health = 10 - finalResult / 4;
            healing = true;
            healingValue = 0.3f;
            speed = 4f;
        }
        // Manejo del roll igual a 20
        if (finalResult == 20)
        {
            colorPrefix = "<color=green>";
            colorSuffix = "</color>";
            numberOfEnemies = 1;
            health = 5;
            attackValue = 1;
            healing = true;
            healingValue = 1f;
            speed = 2f;
        }
        // Texto del resultado del d20
        textMeshProText.text = colorPrefix + finalResultString + colorSuffix;
        // Spawn enemigos
        SpawnEnemy(egg.transform.position, finalResult, numberOfEnemies, health, attackValue, speed);
        // Recupera vida al jugador si healing es true
        if (healing)
        {
            playerScript.HealP(healingValue);
        }
        yield return new WaitForSeconds(2.0f); // Muestra el resultado final por 2 segundos

        // Oculta el texto del roll start
        rollStartTextMeshPro.gameObject.SetActive(false);

        // Oculta el texto del resultado final
        textMeshProText.gameObject.SetActive(false);

        interacting = false;
        // Destruye el huevo interactuado
        Destroy(currentEgg);


        // TODO: Hacer mas cosas si es necesario con los rolls del d20
    }

    // Manejo spawn de enemigos
    void SpawnEnemy(Vector3 spawnPosition, int rollResult, int numberOfEnemies, int health, int attackValue, float speed)
    {
        float angleStep = 360f / numberOfEnemies;

        for (int i = 0; i < numberOfEnemies; i++)
        {
            int terrainLayerMask = 1 << LayerMask.NameToLayer("Terrain");

            float angle = i * angleStep;
            Vector3 offset = Quaternion.Euler(0f, angle, 0f) * Vector3.forward * 2.0f;
            Vector3 enemyPosition = spawnPosition + offset;
            RaycastHit hit;
            if(Physics.Raycast(new Vector3(enemyPosition.x, enemyPosition.y + 100f, enemyPosition.z), new Vector3(0,-1,0), out hit, Mathf.Infinity, terrainLayerMask))
            {
                float terrainHeight = hit.point.y;
                enemyPosition.y = terrainHeight + 2f;
            }

            GameObject enemy = Instantiate(enemyPrefab, enemyPosition, Quaternion.identity);

            EnemyScript enemyScript = enemy.GetComponent<EnemyScript>();

            if (enemyScript != null)
            {
                enemyScript.SetStats(health, attackValue, speed);
            }
        }
    }
}