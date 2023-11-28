using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public GameObject player;
    public GameObject arrow;
    public float arrowShowDistance = 50f;
    
    private void Update()
    {
        // Find the nearest egg
        GameObject nearestEgg = FindNearestEgg();

        if (nearestEgg != null)
        {
            // Calculate direction to the nearest egg
            Vector3 directionToEgg = nearestEgg.transform.position - player.transform.position;
            directionToEgg.y = 0f; // Ignore the vertical component

            // Rotate the arrow to point towards the egg
            arrow.transform.rotation = Quaternion.LookRotation(directionToEgg) * Quaternion.Euler(90f, 0f, 0f);

            // Show or hide the arrow based on distance
            arrow.SetActive(directionToEgg.magnitude <= arrowShowDistance);
        }
        else
        {
            // No eggs found, hide the arrow
            arrow.SetActive(false);
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private GameObject FindNearestEgg()
    {
        GameObject[] eggs = GameObject.FindGameObjectsWithTag("Egg");

        if (eggs.Length == 0)
        {
            // No eggs found
            return null;
        }

        GameObject nearestEgg = eggs[0];
        float minDistance = Vector3.Distance(player.transform.position, nearestEgg.transform.position);

        for (int i = 1; i < eggs.Length; i++)
        {
            float distance = Vector3.Distance(player.transform.position, eggs[i].transform.position);

            if (!(distance < minDistance)) continue;
            nearestEgg = eggs[i];
            minDistance = distance;
        }

        return nearestEgg;
    }
}
