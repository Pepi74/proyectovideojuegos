using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lily_spawner : MonoBehaviour
{
    public GameObject plane;
    public GameObject lilyPrefab;
    public int MaxPads = 200;
    // Start is called before the first frame update
    void Start()
    {
        Renderer planeRenderer = plane.GetComponent<Renderer>();
        // Get the bounds of the plane in world space
        Bounds planeBounds = planeRenderer.bounds;
        // Get the X and Z coordinates of the edges
        float minX = planeBounds.min.x;
        float maxX = planeBounds.max.x;

        float minZ = planeBounds.min.z;
        float maxZ = planeBounds.max.z;

        float randomPosX;
        float randomPosZ;
        
        for (int i = 0; i < MaxPads; i++) {
            randomPosX = Random.Range(minX, maxX);
            randomPosZ = Random.Range(minZ, maxZ);
            Instantiate(lilyPrefab, new Vector3(randomPosX, plane.transform.position.y + 2f, randomPosZ), Quaternion.identity);
        }
        //Instantiate(lilyPrefab, spawnPosition + new Vector3(0f, 2f, 0f), Quaternion.identity);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
