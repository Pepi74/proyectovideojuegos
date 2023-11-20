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
            float x = Random.Range(0f, terrainData.size.x);
            float z = Random.Range(0f, terrainData.size.z);

            // Convert the world coordinates to terrain coordinates
            float normalizedX = x / terrainData.size.x;
            float normalizedZ = z / terrainData.size.z;

            // Create a TreeInstance
            TreeInstance treeInstance = new TreeInstance();
            treeInstance.position = new Vector3(normalizedX, 0.2f, normalizedZ);
            treeInstance.widthScale = 1f;
            treeInstance.heightScale = 1f;
            treeInstance.color = Color.white;
            treeInstance.lightmapColor = Color.white;

            // Add the TreeInstance to the terrain data
            terrainData.treeInstances = AppendToArray(terrainData.treeInstances, treeInstance);
        }

        // Refresh the terrain to see the changes
        terrain.Flush();
    }

    TreeInstance[] AppendToArray(TreeInstance[] array, TreeInstance item)
    {
        TreeInstance[] newArray = new TreeInstance[array.Length + 1];
        array.CopyTo(newArray, 0);
        newArray[array.Length] = item;
        return newArray;
    }

    private void OnApplicationQuit()
    {
        if (terrain == null)
        {
            Debug.LogError("Terrain not assigned!");
            return;
        }

        RemoveAllTrees();
    }

    void RemoveAllTrees()
    {
        TerrainData terrainData = terrain.terrainData;

        // Clear the treeInstances array
        terrainData.treeInstances = new TreeInstance[0];

        // Refresh the terrain to see the changes
        terrain.Flush();
    }

}
