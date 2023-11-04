using UnityEngine;

public class EggSpawner : MonoBehaviour
{
    public GameObject eggPrefab; // Prefab de los huevos
    public Terrain terrain; // Referencia al terreno

    public int minSpawn = 100; // Cantidad minima de huevos que spawnearan
    public int maxSpawn = 200; // Cantidad maxima de huevos que spawnearan

    // Manejo de spawn de huevos alrededor del terreno
    void Start()
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
}
