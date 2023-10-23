using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public int width = 256;
    public int length = 256;
    public float scale = 20.0f;

    void Start()
    {
        GenerateTerrain();
    }

    void GenerateTerrain()
    {
        Terrain terrain = GetComponent<Terrain>();
        terrain.terrainData = GenerateTerrain(terrain.terrainData);
    }

    TerrainData GenerateTerrain(TerrainData terrainData)
    {
        terrainData.heightmapResolution = width + 1;
        terrainData.size = new Vector3(width, 5, length);
        terrainData.SetHeights(0, 0, GenerateHeights());
        return terrainData;
    }

    float[,] GenerateHeights()
    {
        float[,] heights = new float[width, length];
        Vector2 offset = new Vector2(Random.Range(0, 9999), Random.Range(0, 9999));

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < length; y++)
            {
                heights[x, y] = Mathf.PerlinNoise((x + offset.x) / scale, (y + offset.y) / scale);
            }
        }

        return heights;
    }
}
