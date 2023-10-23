using UnityEngine;

public class EggSpawner : MonoBehaviour
{
    public GameObject eggPrefab; // Reference to your egg prefab.
    public Terrain terrain; // Reference to your terrain.

    void Start()
    {
        // Get the terrain data.
        TerrainData terrainData = terrain.terrainData;

        // Spawn eggs at random positions on the terrain.
        for (int i = 0; i < Random.Range(300, 501); i++)
        {
            float randomX = Random.Range(0f, terrainData.size.x);
            float randomZ = Random.Range(0f, terrainData.size.z);

            // Sample the height at the random position on the terrain.
            float terrainHeight = terrain.SampleHeight(new Vector3(randomX, 0, randomZ));

            Vector3 spawnPosition = new Vector3(randomX, terrainHeight + 0.5f, randomZ);
            Instantiate(eggPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
