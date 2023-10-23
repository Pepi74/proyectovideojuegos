using System.Collections;
using UnityEngine;
using TMPro;

public class EggInteraction : MonoBehaviour
{
    public float interactionRange = 2.0f;
    public TextMeshProUGUI interactionText; // Reference to the TextMeshPro Text GameObject.
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI rollStartText;
    public GameObject enemyPrefab;
    public GameObject currentEgg; // This field will store the reference to the egg being interacted with.
    public PlayerScript playerScript;
    [SerializeField]
    private bool interacting = false;

    public LayerMask eggLayer;

    void Start()
    {
        // Make sure the TextMeshPro Text is initially disabled.
        interactionText.gameObject.SetActive(false);
    }

    void Update()
    {
        /* Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionRange))
        {
            if (hit.collider.CompareTag("Egg") && !interacting)
            {
                ShowInteractionText();
                if (Input.GetKeyDown(KeyCode.E))
                {
                    // Perform interaction logic when "E" is pressed.
                    interacting = true;
                    HandleInteraction(hit.collider.gameObject);
                }
            }
            else
            {
                HideInteractionText();
            }
        }
        else
        {
            HideInteractionText();
        } */

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
                        // Perform interaction logic when "E" is pressed.
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
        // Show the interaction text.
        interactionText.gameObject.SetActive(true);
    }

    void HideInteractionText()
    {
        // Hide the interaction text.
        interactionText.gameObject.SetActive(false);
    }

    void HandleInteraction(GameObject egg)
    {
        //Debug.Log("Interaction with egg successful!");
        // Implement your logic for the interaction (e.g., rolling a D20 and spawning enemies).

        currentEgg = egg; // Assign the current egg reference.

        rollStartText.text = "Rolling D20...";
        rollStartText.gameObject.SetActive(true);

        // Show random numbers for 2 seconds.
        StartCoroutine(DisplayRandomNumbersAndResult(egg));
    }

    int RollD20()
    {
        return Random.Range(1, 21);
    }

    IEnumerator DisplayRandomNumbersAndResult(GameObject egg)
    {
        int randomNumbersDuration = 2; // Display random numbers for 2 seconds.
        float endTime = Time.time + randomNumbersDuration;
        TextMeshProUGUI textMeshProText = resultText;
        TextMeshProUGUI rollStartTextMeshPro = rollStartText;

        textMeshProText.gameObject.SetActive(true);
        // Display random numbers for the specified duration.
        while (Time.time < endTime)
        {
            int randomValue = Random.Range(1, 21);
            textMeshProText.text = randomValue.ToString();
            yield return new WaitForSeconds(0.1f);
        }

        // Show the final result.
        int finalResult = RollD20();
        string finalResultString = finalResult.ToString();
        string colorPrefix = "";
        string colorSuffix = "";
        int numberOfEnemies = 0;
        int attackValue = 0;
        int health = 0;
        bool healing = false;
        float healingValue = 0f;
        float speed = 0f;
        if (finalResult == 1)
        {
            colorPrefix = "<color=red>";
            colorSuffix = "</color>";
            numberOfEnemies = 15;
            attackValue = 5;
            health = 20;
            speed = 10f;
        }
        if (finalResult > 1 && finalResult <= 7)
        {
            colorPrefix = "<color=orange>";
            colorSuffix = "</color>";
            numberOfEnemies = Random.Range(10, 15 - finalResult / 2);
            attackValue = 4;
            health = 20 - finalResult / 2;
            speed = 8f;
        }
        if (finalResult > 7 && finalResult <= 13)
        {
            colorPrefix = "<color=yellow>";
            colorSuffix = "</color>";
            numberOfEnemies = Random.Range(5, 10 - finalResult / 4);
            attackValue = 3;
            health = 15 - finalResult / 3;
            speed = 6f;
        }
        if (finalResult > 13 && finalResult <= 19)
        {
            colorPrefix = "<color=blue>";
            colorSuffix = "</color>";
            numberOfEnemies = Random.Range(2, 5 - finalResult / 8);
            attackValue = 2;
            health = 10 - finalResult / 4;
            healing = true;
            healingValue = 0.3f;
            speed = 4f;
        }
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
        textMeshProText.text = colorPrefix + finalResultString + colorSuffix;
        SpawnEnemy(egg.transform.position, finalResult, numberOfEnemies, health, attackValue, speed);
        if (healing)
        {
            playerScript.HealP(healingValue);
        }
        yield return new WaitForSeconds(2.0f); // Display the final result for 2 seconds.

        // Hide the roll start message.
        rollStartTextMeshPro.gameObject.SetActive(false);

        // Hide the result text.
        textMeshProText.gameObject.SetActive(false);

        interacting = false;
        // Destroy the current egg GameObject.
        Destroy(currentEgg);


        // Handle other outcomes (e.g., spawning enemies) based on the final result.
    }

    void SpawnEnemy(Vector3 spawnPosition, int rollResult, int numberOfEnemies, int health, int attackValue, float speed)
    {
        // Calculate the angle between each enemy in the circle.
        float angleStep = 360f / numberOfEnemies;

        for (int i = 0; i < numberOfEnemies; i++)
        {
            // Calculate the position for each enemy in the circle.
            int terrainLayerMask = 1 << LayerMask.NameToLayer("Terrain");

            float angle = i * angleStep;
            Vector3 offset = Quaternion.Euler(0f, angle, 0f) * Vector3.forward * 2.0f; // Adjust the 2.0f for desired radius.
            Vector3 enemyPosition = spawnPosition + offset;
            RaycastHit hit;
            if(Physics.Raycast(new Vector3(enemyPosition.x, enemyPosition.y + 100f, enemyPosition.z), new Vector3(0,-1,0), out hit, Mathf.Infinity, terrainLayerMask))
            {
                float terrainHeight = hit.point.y;
                enemyPosition.y = terrainHeight + 0.5f;
            }

            // Instantiate the enemy prefab.
            GameObject enemy = Instantiate(enemyPrefab, enemyPosition, Quaternion.identity);

            // Get the enemy's script for customization.
            EnemyScript enemyScript = enemy.GetComponent<EnemyScript>();

            if (enemyScript != null)
            {
                enemyScript.SetStats(health, attackValue, speed);
            }
        }
    }
}