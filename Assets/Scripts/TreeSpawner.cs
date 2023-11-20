using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    public Terrain terrain; // Assign your terrain in the Unity Editor
    public GameObject treePrefab; // The tree prefab you want to add
    public int numberOfTrees = 100;

    void Start()
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

        for (int i = 0; i < numberOfTrees; i++) {

            // Choose a position for the tree within the terrain size
            float randomX = Random.Range(0f, terrainData.size.x);
            float randomZ = Random.Range(0f, terrainData.size.z);

            float terrainHeight = terrain.SampleHeight(new Vector3(randomX, 0, randomZ));

            Vector3 spawnPosition = new Vector3(randomX, terrainHeight + 0.5f, randomZ);
            Instantiate(treePrefab, spawnPosition, Quaternion.identity);
        }

        // Refresh the terrain to see the changes
        //terrain.Flush();
    }

}
