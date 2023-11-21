using UnityEngine;
using UnityEngine.Events;

public class PlayerSpawnEvent : UnityEvent<GameObject> { }

public class Spawner : MonoBehaviour
{
    public GameObject eggPrefab; // Prefab de los huevos
    public GameObject playerPrefab;
    public Terrain terrain; // Referencia al terreno
    public PlayerSpawnEvent onPlayerSpawned = new PlayerSpawnEvent();
    public GameObject boundaryPrefab;

    public int minSpawn = 100; // Cantidad minima de huevos que spawnearan
    public int maxSpawn = 200; // Cantidad maxima de huevos que spawnearan

    public GameObject water;
    public GameObject lilyPrefab;
    public int MaxPads = 200;

    public GameObject treePrefab; // The tree prefab you want to add
    public int numberOfTrees = 100;

    // Manejo de spawn de huevos alrededor del terreno y player en el centro del terreno
    void Start()
    {
        SpawnBoundaries();
        SpawnPlayer();
        SpawnEggs();
        SpawnLilyPads();
        SpawnTrees();
    }

    void SpawnEggs()
    {
        TerrainData terrainData = terrain.terrainData;

        for (int i = 0; i < Random.Range(minSpawn, maxSpawn + 1); i++)
        {
            float randomX = Random.Range(0f, terrainData.size.x - 2f);
            float randomZ = Random.Range(0f, terrainData.size.z - 2f);

            float terrainHeight = terrain.SampleHeight(new Vector3(randomX, 0, randomZ));

            Vector3 spawnPosition = new Vector3(randomX, terrainHeight + 0.5f, randomZ);
            Instantiate(eggPrefab, spawnPosition, Quaternion.identity);
        }
    }

    void SpawnPlayer()
    {
        TerrainData terrainData = terrain.terrainData;

        float spawnX = terrainData.size.x / 2;
        float spawnZ = terrainData.size.z / 2;
        float terrainHeight = terrain.SampleHeight(new Vector3(spawnX, 0, spawnZ));
        Vector3 spawnPosition = new Vector3(spawnX, terrainHeight + 2, spawnZ);
        GameObject player = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
        onPlayerSpawned.Invoke(player);
    }

    void SpawnBoundaries()
    {
        TerrainData terrainData = terrain.terrainData;
        Vector3 terrainSize = terrain.terrainData.size;

        float[] terrainHeights =
        {
            terrain.SampleHeight(new Vector3(0, 0, terrainSize.z / 2)),
            terrain.SampleHeight(new Vector3(terrainSize.x, 0, terrainSize.z / 2)),
            terrain.SampleHeight(new Vector3(terrainSize.x / 2, 0, 0)),
            terrain.SampleHeight(new Vector3(terrainSize.x / 2, 0, terrainSize.z))
        };

        Vector3[] boundaryPositions =
        {
            new Vector3(0, terrainHeights[0], terrainSize.z / 2), // Right side
            new Vector3(terrainSize.x, terrainHeights[1], terrainSize.z / 2), // Left side
            new Vector3(terrainSize.x / 2, terrainHeights[2], 0), // Top side
            new Vector3(terrainSize.x / 2, terrainHeights[3], terrainSize.z) // Bottom side
        };

        foreach (Vector3 position in boundaryPositions)
        {
            GameObject boundary = Instantiate(boundaryPrefab, position, Quaternion.identity);
            if (position.z != terrainSize.z / 2) boundary.transform.localScale = new Vector3(terrainSize.x, 50f, 4);
            else
                boundary.transform.localScale = new Vector3(4, 50f, terrainSize.z);
        }
    }

    void SpawnLilyPads()
    {
        Renderer waterRenderer = water.GetComponent<Renderer>();
        // Get the bounds of the water in world space
        Bounds waterBounds = waterRenderer.bounds;
        // Get the X and Z coordinates of the edges
        float minX = waterBounds.min.x;
        float maxX = waterBounds.max.x;

        float minZ = waterBounds.min.z;
        float maxZ = waterBounds.max.z;

        float randomPosX;
        float randomPosZ;

        for (int i = 0; i < MaxPads; i++)
        {
            randomPosX = Random.Range(minX, maxX);
            randomPosZ = Random.Range(minZ, maxZ);
            Instantiate(lilyPrefab, new Vector3(randomPosX, water.transform.position.y + 2f, randomPosZ),
                Quaternion.identity);
        }
    }

    void SpawnTrees()
    {
        if (terrain == null || treePrefab == null)
        {
            Debug.LogError("Terrain or treePrefab not assigned!");
            return;
        }

        AddTree();
    }

    void AddTree()
    {
        TerrainData terrainData = terrain.terrainData;

        for (int i = 0; i < numberOfTrees; i++)
        {

            // Choose a position for the tree within the terrain size
            float randomX = Random.Range(0f, terrainData.size.x);
            float randomZ = Random.Range(0f, terrainData.size.z);

            float terrainHeight = terrain.SampleHeight(new Vector3(randomX, 0, randomZ));

            Vector3 spawnPosition = new Vector3(randomX, terrainHeight + 0.5f, randomZ);
            Instantiate(treePrefab, spawnPosition, Quaternion.identity);
        }
    }

}