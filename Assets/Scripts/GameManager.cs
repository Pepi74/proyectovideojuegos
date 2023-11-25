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

    private void Start()
    {
        roundNumber = 1;
        terrainGenerator.GenerateTerrain(terrain.terrainData);
        spawner.SpawnBoundaries();
        spawner.SpawnPlayer();
        spawner.SpawnEggs();
        spawner.SpawnLilyPads();
        spawner.SpawnTrees();
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
    }

    private void Update()
    {
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
        NextRoundChanges();
        ReSpawnObjects();
        reSpawning = false;
        roundNumber++;
        roundText.text = "Round: " + roundNumber;
        eggInteraction.enemyLevel++;
        if (roundNumber % 2 == 1 && roundNumber != 1) playerScript.upgradePoints++;
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
        DestroyPreviousObject("Boundary");
        terrainGenerator.GenerateTerrain(terrain.terrainData);
        spawner.SpawnBoundaries();
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
}
