using UnityEngine;
using UnityEngine.Events;

public class PlayerSpawnEvent : UnityEvent<GameObject> { }

public class Spawner : MonoBehaviour
{
    public GameObject eggPrefab; // Prefab de los huevos
    public GameObject playerPrefab;
    public Terrain terrain; // Referencia al terreno
    public readonly PlayerSpawnEvent onPlayerSpawned = new PlayerSpawnEvent();
    public GameObject boundaryPrefab;

    public int minSpawn; // Cantidad minima de huevos que spawnearan
    public int maxSpawn; // Cantidad maxima de huevos que spawnearan

    public GameObject water;
    public GameObject lilyPrefab;
    public int maxPads;

    public GameObject treePrefab; // The tree prefab you want to add
    public int numberOfTrees;

    // Manejo de spawn de huevos alrededor del terreno y player en el centro del terreno
    public void SpawnEggs()
    {
        TerrainData terrainData = terrain.terrainData;

        for (int i = 0; i < Random.Range(minSpawn, maxSpawn + 1); i++)
        {
            float randomX = Random.Range(0f, terrainData.size.x - 2f);
            float randomZ = Random.Range(0f, terrainData.size.z - 2f);

            float terrainHeight = terrain.SampleHeight(new Vector3(randomX, 0, randomZ));
            while (terrainHeight  + 0.5f < water.transform.position.y)
            {
                randomX = Random.Range(0f, terrainData.size.x - 2f);
                randomZ = Random.Range(0f, terrainData.size.z - 2f);
                terrainHeight = terrain.SampleHeight(new Vector3(randomX, 0, randomZ));
            }

            Vector3 spawnPosition = new Vector3(randomX, terrainHeight + 0.5f, randomZ);
            Instantiate(eggPrefab, spawnPosition, Quaternion.identity);
        }
    }

    public void SpawnPlayer()
    {
        TerrainData terrainData = terrain.terrainData;

        float spawnX = terrainData.size.x / 2;
        float spawnZ = terrainData.size.z / 2;
        float terrainHeight = terrain.SampleHeight(new Vector3(spawnX, 0, spawnZ));
        Vector3 spawnPosition = new Vector3(spawnX, terrainHeight + 3f, spawnZ);
        GameObject player = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
        onPlayerSpawned.Invoke(player);
    }

    public void SpawnBoundaries()
    {
        var data = terrain.terrainData;
        Vector3 terrainSize = data.size;

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
            boundary.transform.localScale = position.z != terrainSize.z / 2 ? new Vector3(terrainSize.x, 50f, 4) : new Vector3(4, 50f, terrainSize.z);
        }
    }

    public void SpawnLilyPads()
    {
        Renderer waterRenderer = water.GetComponent<Renderer>();
        // Get the bounds of the water in world space
        Bounds waterBounds = waterRenderer.bounds;
        // Get the X and Z coordinates of the edges
        float minX = waterBounds.min.x;
        float maxX = waterBounds.max.x;

        float minZ = waterBounds.min.z;
        float maxZ = waterBounds.max.z;

        int terrainLayerMask = 1 << LayerMask.NameToLayer("Terrain");

        for (int i = 0; i < maxPads; i++)
        {
            var randomPosX = Random.Range(minX, maxX);
            var randomPosZ = Random.Range(minZ, maxZ);
            while (Physics.Raycast(new Vector3(randomPosX, water.transform.position.y + 0.1f, randomPosZ), Vector3.up, out _, Mathf.Infinity, terrainLayerMask))
            {
                randomPosX = Random.Range(minX, maxX);
                randomPosZ = Random.Range(minZ, maxZ);
            }
            Instantiate(lilyPrefab, new Vector3(randomPosX, water.transform.position.y + 0.1f, randomPosZ),
                Quaternion.identity);
        }
    }

    public void SpawnTrees()
    {
        if (terrain == null || treePrefab == null)
        {
            Debug.LogError("Terrain or treePrefab not assigned!");
            return;
        }

        AddTree();
    }

    private void AddTree()
    {
        TerrainData terrainData = terrain.terrainData;

        for (int i = 0; i < numberOfTrees; i++)
        {

            // Choose a position for the tree within the terrain size
            float randomX = Random.Range(0f, terrainData.size.x);
            float randomZ = Random.Range(0f, terrainData.size.z);

            float terrainHeight = terrain.SampleHeight(new Vector3(randomX, 0, randomZ));

            Vector3 spawnPosition = new Vector3(randomX, terrainHeight, randomZ);
            Instantiate(treePrefab, spawnPosition, Quaternion.identity);
        }
    }

}