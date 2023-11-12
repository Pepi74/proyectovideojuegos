using UnityEngine;
using UnityEngine.Events;

public class PlayerSpawnEvent : UnityEvent<GameObject> { }

public class Spawner : MonoBehaviour
{
    public GameObject eggPrefab; // Prefab de los huevos
    public GameObject playerPrefab;
    public Terrain terrain; // Referencia al terreno
    public PlayerSpawnEvent onPlayerSpawned = new PlayerSpawnEvent();

    public int minSpawn = 100; // Cantidad minima de huevos que spawnearan
    public int maxSpawn = 200; // Cantidad maxima de huevos que spawnearan

    // Manejo de spawn de huevos alrededor del terreno y player en el centro del terreno
    void Start()
    {
        SpawnEggs();
        SpawnPlayer();
    }

    void SpawnEggs()
    {
        TerrainData terrainData = terrain.terrainData;

        for (int i = 0; i < Random.Range(minSpawn, maxSpawn+1); i++)
        {
            float randomX = Random.Range(0f, terrainData.size.x);
            float randomZ = Random.Range(0f, terrainData.size.z);

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
}
