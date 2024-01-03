using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallProtection : MonoBehaviour
{
    [SerializeField] private float height = 10f;
    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Enemy")){
            other.transform.position = new Vector3(other.transform.position.x, other.transform.position.y + height, other.transform.position.z);
        }
    }
}
