using System.Collections;
using Enemigos;
using UnityEngine;
using TMPro;

// TODO: mejorar este script quizas?
public class EggInteraction : MonoBehaviour
{
    public float interactionRange = 2.0f; // Rango de interaccion de huevos
    public TextMeshProUGUI interactionText; // Texto de ui interaccion
    public TextMeshProUGUI resultText; // Texto de resultado d20
    public TextMeshProUGUI rollStartText; // Texto "Rolling D20...", iniciado en HandleInteraction
    public GameObject currentEgg; // Huevo que se interactua
    public PlayerScript playerScript; // Componente PlayerScript del jugador
    [SerializeField]
    private bool interacting; // Booleano que indica si se esta interactuando o no

    public LayerMask eggLayer; // Layer del huevo

    public int enemyLevel = 1;

    public int forcedFinalResult; // Para testear cosas y forzar el resultado final

    public bool forceResult;
    public GameObject duckPrefab;
    public GameObject crocPrefab;
    public GameObject porcupinePrefab;
    public GameObject skunkPrefab;

    public int roundNumber;
    public GameManager gameManager;

    private void Start()
    {
        interacting = false;
        interactionText = GameObject.Find("InteractText").GetComponent<TextMeshProUGUI>();
        interactionText.text = "Press <color=yellow>E</color> to interact";
        interactionText.gameObject.SetActive(false);
        rollStartText = GameObject.Find("RollStartText").GetComponent<TextMeshProUGUI>();
        rollStartText.text = "Rolling D20...";
        rollStartText.gameObject.SetActive(false);
        resultText = GameObject.Find("ResultText").GetComponent<TextMeshProUGUI>();
        resultText.gameObject.SetActive(false);
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        roundNumber = gameManager.roundNumber;
    }

    private void Update()
    {
        //Spherecast que detecta huevos cercanos al jugador
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, interactionRange, eggLayer);

        if (hitColliders.Length > 0)
        {

            foreach (Collider collider1 in hitColliders)
            {
                if (collider1.CompareTag("Egg") && !interacting)
                {
                    ShowInteractionText();
                    if (!Input.GetKeyDown(KeyCode.E)) continue;
                    // Cuando se presiona "E", se maneja la interaccion
                    interacting = true;
                    HandleInteraction(collider1.gameObject);
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

    private void ShowInteractionText()
    {
        // Muestra texto interaccion
        interactionText.gameObject.SetActive(true);
    }

    private void HideInteractionText()
    {
        // Oculta texto interaccion
        interactionText.gameObject.SetActive(false);
    }

    private void HandleInteraction(GameObject egg)
    {
        // Manejo de interaccion

        currentEgg = egg; // Asigna el huevo interactuado a la variable

        rollStartText.gameObject.SetActive(true); // Muestra el texto "Rolling D20..."

        // Corutina del manejo del d20
        StartCoroutine(DisplayRandomNumbersAndResult(egg));
    }

    // Numero entero random entre 1 y 20
    private static int RollD20()
    {
        return Random.Range(1, 21);
    }

    // Manejo del d20 y resultados dependiendo del roll del d20
    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator DisplayRandomNumbersAndResult(GameObject egg)
    {
        const int randomNumbersDuration = 2; // Duracion de numeros random antes del resultado final
        float endTime = Time.time + randomNumbersDuration;
        TextMeshProUGUI textMeshProText = resultText;
        textMeshProText.color = Color.white;
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
        if (forceResult) finalResult = forcedFinalResult; // Para testear valores del 20 forzados
        string finalResultString = finalResult.ToString();
        // Iniciacion de variables que cambiaran dependiendo del resultado final del d20
        int numberOfEnemies = 0;
        bool healing = false;
        float healingValue = 0f;
        float speed = 0f;
        bool playerLevelUp = false;
        switch (finalResult)
        {
            // Manejo del roll igual a 1
            case 1:
                enemyLevel++;
                textMeshProText.color = Color.red;
                numberOfEnemies = 6 + (roundNumber / 4);
                speed = 9f;
                break;
            // Manejo del roll entre 2 y 7
            case > 1 and <= 7:
                textMeshProText.color = new Color(1.0f, 0.44f, 0.0f);
                numberOfEnemies = Random.Range(4, 6 + (roundNumber / 4));
                speed = 7f;
                break;
            // Manejo del roll entre 8 y 13
            case > 7 and <= 13:
                textMeshProText.color = Color.yellow;
                numberOfEnemies = Random.Range(3, 5 + (roundNumber / 4));
                speed = 6f;
                break;
            // Manejo del roll entre 14 y 19
            case > 13 and <= 19:
                textMeshProText.color = Color.blue;
                numberOfEnemies = Random.Range(2, 4 + (roundNumber / 4));
                speed = 4f;
                healing = true;
                healingValue = finalResult switch
                {
                    14 => 0.15f,
                    15 => 0.20f,
                    16 => 0.25f,
                    17 => 0.30f,
                    18 => 0.35f,
                    _ => 0.40f
                };
                break;
            // Manejo del roll igual a 20
            case 20:
                textMeshProText.color = Color.green;
                numberOfEnemies = 1;
                speed = 2f;
                playerLevelUp = true;
                break;
        }

        int attackValue = 1 * enemyLevel;
		int health = (2 * enemyLevel) - (finalResult / 2);
        // Texto del resultado del d20
        textMeshProText.text = finalResultString;
        // Spawn enemigos
        SpawnEnemies(egg.transform.position, numberOfEnemies, health, attackValue, speed);
        // Recupera vida al jugador si healing es true
        if (healing) playerScript.HealP(healingValue);

        if (playerLevelUp) playerScript.LevelUp();
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
    public void SpawnEnemies(Vector3 spawnPosition, int numberOfEnemies, int health, int attackValue, float speed)
    {
        float angleStep = 360f / numberOfEnemies;

        for (int i = 0; i < numberOfEnemies; i++)
        {
            int terrainLayerMask = 1 << LayerMask.NameToLayer("Terrain");

            float angle = i * angleStep;
            Vector3 offset = Quaternion.Euler(0f, angle, 0f) * Vector3.forward * 2.0f;
            Vector3 enemyPosition = spawnPosition + offset;
            if(Physics.Raycast(new Vector3(enemyPosition.x, enemyPosition.y + 100f, enemyPosition.z), new Vector3(0,-1,0), out var hit, Mathf.Infinity, terrainLayerMask))
            {
                float terrainHeight = hit.point.y;
                enemyPosition.y = terrainHeight + 2f;
            }

            int rngEnemy = Random.Range(1, 101);

            switch (rngEnemy)
            {
                case <= 25:
                {
                    GameObject enemy = Instantiate(duckPrefab, enemyPosition, Quaternion.identity);
                    Pato pato = enemy.GetComponent<Pato>();
                    pato.SetStats(health, attackValue, speed, enemyLevel);
                    break;
                }
                case <= 50:
                {
                    GameObject enemy = Instantiate(crocPrefab, enemyPosition, Quaternion.identity);
                    Cocodrilo cocodrilo = enemy.GetComponent<Cocodrilo>();
                    cocodrilo.SetStats(health, attackValue, speed, enemyLevel);
                    break;
                }
                case <= 75:
                {
                    GameObject enemy = Instantiate(porcupinePrefab, enemyPosition, Quaternion.identity);
                    Puercospin puercospin = enemy.GetComponent<Puercospin>();
                    puercospin.SetStats(health, attackValue, speed, enemyLevel);
                    break;
                }
                case <= 100:
                {
                    GameObject enemy = Instantiate(skunkPrefab, enemyPosition, Quaternion.identity);
                    Zorrillo zorrillo = enemy.GetComponent<Zorrillo>();
                    zorrillo.SetStats(health, attackValue, speed, enemyLevel);
                    break;
                }
            }
        }
    }
}