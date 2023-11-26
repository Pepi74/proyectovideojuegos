using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int roundNumber;
    public Spawner spawner;
    public Terrain terrain;
    public TerrainGenerator terrainGenerator;
    public bool reSpawning;
    public GameObject player;
    public EggInteraction eggInteraction;
    public TextMeshProUGUI roundText;
    public PlayerScript playerScript;
    public UpgradeManager upgradeManager;
    public TextMeshProUGUI nextRoundText;
    public bool bossRound;
    public GameObject boss;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI rollStartText;
    
    private void Start()
    {
        roundNumber = 1;
        terrainGenerator.GenerateTerrain(terrain.terrainData);
        //spawner.SpawnBoundaries();
        spawner.SpawnPlayer();
        spawner.SpawnEggs();
        spawner.SpawnLilyPads();
        spawner.SpawnTrees();
        spawner.SpawnGrass();
        spawner.SpawnRocks();
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerScript>();
        eggInteraction = player.GetComponent<EggInteraction>();
        roundText = GameObject.Find("RoundText").GetComponent<TextMeshProUGUI>();
        roundText.color = Color.red;
        roundText.text = "Round: " + roundNumber;
        upgradeManager = GameObject.Find("UI").GetComponent<UpgradeManager>();
        upgradeManager.RandomizeUpgrades();
        nextRoundText = GameObject.Find("NextRoundText").GetComponent<TextMeshProUGUI>();
        nextRoundText.gameObject.SetActive(false);
        bossRound = false;
    }

    private void Update()
    {
        if (bossRound)
        {
            if (boss != null) return;
            bossRound = false;
            playerScript.upgradePoints += 2;
            return;
        }
        
        if (!AreAllEnemiesAndEggsDestroyed() || reSpawning) return;
        reSpawning = true;
        StartCoroutine(StartRoundCountdown(5f));
        StartCoroutine(NextRound());
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    private static bool AreAllEnemiesAndEggsDestroyed()
    {
        // Check if there are no more enemies or eggs in the scene
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] eggs = GameObject.FindGameObjectsWithTag("Egg");

        return enemies.Length == 0 && eggs.Length == 0;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator NextRound()
    {
        yield return new WaitForSeconds(5f);
        roundNumber++;
        roundText.text = "Round: " + roundNumber;
        eggInteraction.enemyLevel++;
        NextRoundChanges();
        if (roundNumber % 5 == 0)
        {
            BossObjects();
        }
        else
        {
            ReSpawnObjects();
            reSpawning = false;
            if (roundNumber % 2 == 1 && !bossRound) playerScript.upgradePoints++;
        }
    }

    private void NextRoundChanges()
    {
        ReGenerateTerrain();
        MovePlayer();
    }

    private void ReGenerateTerrain()
    {
        DestroyPreviousObject("Tree");
        DestroyPreviousObject("Lily");
        //DestroyPreviousObject("Boundary");
        DestroyPreviousObject("Grass");
        DestroyPreviousObject("Rock");
        terrainGenerator.GenerateTerrain(terrain.terrainData);
        //spawner.SpawnBoundaries();
    }

    private static void DestroyPreviousObject(string objectTag)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(objectTag);
        foreach (GameObject gObject in objects)
        {
            Destroy(gObject);
        }
    }
    
    private void MovePlayer()
    {
        TerrainData terrainData = terrain.terrainData;
        float spawnX = terrainData.size.x / 2;
        float spawnZ = terrainData.size.z / 2;
        float terrainHeight = terrain.SampleHeight(new Vector3(spawnX, 0, spawnZ));
        Vector3 spawnPosition = new Vector3(spawnX, terrainHeight + 3f, spawnZ);
        player.transform.position = spawnPosition;
    }

    private void ReSpawnObjects()
    {
        spawner.SpawnEggs();
        spawner.SpawnLilyPads();
        spawner.SpawnTrees();
        spawner.SpawnGrass();
        spawner.SpawnRocks();
    }

    private void BossObjects()
    {
        spawner.SpawnLilyPads();
        spawner.SpawnTrees();
        spawner.SpawnGrass();
        spawner.SpawnRocks();
        StartCoroutine(RollD20BeforeBoss());
    }
    
    private IEnumerator StartRoundCountdown(float countdownDuration)
    {
        float timer = countdownDuration;
        nextRoundText.gameObject.SetActive(true);
        while (timer > 0f)
        {
            UpdateRoundCountdownText(timer);
            yield return new WaitForSeconds(1f);  // Wait for one second
            timer--;
        }
        
        nextRoundText.gameObject.SetActive(false);
    }
    
    private void UpdateRoundCountdownText(float timeRemaining)
    {
        nextRoundText.text = "Next round in: " + ((int) timeRemaining);
    }
    
    private static int RollD20()
    {
        return Random.Range(1, 21);
    }
    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator RollD20BeforeBoss()
    {
        const int randomNumbersDuration = 2; // Duracion de numeros random antes del resultado final
        float endTime = Time.time + randomNumbersDuration;
        resultText.color = Color.white;
        rollStartText.gameObject.SetActive(true);

        resultText.gameObject.SetActive(true);
        // Muestra numeros random antes del resultado final del d20.
        while (Time.time < endTime)
        {
            int randomValue = Random.Range(1, 21);
            resultText.text = randomValue.ToString();
            yield return new WaitForSeconds(0.1f);
        }

        // Resultado final.
        int finalResult = RollD20();
        string finalResultString = finalResult.ToString();
        // Iniciacion de variables que cambiaran dependiendo del resultado final del d20
        resultText.color = finalResult switch
        {
            // Manejo del roll igual a 1
            1 => Color.red,
            // Manejo del roll entre 2 y 7
            > 1 and <= 7 => new Color(1.0f, 0.44f, 0.0f),
            // Manejo del roll entre 8 y 13
            > 7 and <= 13 => Color.yellow,
            // Manejo del roll entre 14 y 19
            > 13 and <= 19 => Color.blue,
            // Manejo del roll igual a 20
            20 => Color.green,
            _ => resultText.color
        };

        resultText.text = finalResultString;
        int attackValue = 1 * eggInteraction.enemyLevel;
		int health = (2 * eggInteraction.enemyLevel) - (finalResult / 2);
        spawner.SpawnBoss(health, attackValue, eggInteraction.enemyLevel);
        boss = GameObject.FindGameObjectWithTag("Boss");
        bossRound = true;
        reSpawning = false;
        yield return new WaitForSeconds(2.0f);
        resultText.gameObject.SetActive(false);
        rollStartText.gameObject.SetActive(false);
    }
}
