using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToungeTip : MonoBehaviour
{
    public float tipSpeed = 10.0f;
    public float maxDistance = 10.0f;
    public float returnSpeed = 15.0f;
    public int attackValue;
    public LineRenderer lr;
    public string playerTag = "Player"; // Specify the player tag
    public LayerMask terrainLayer;
    private Vector3 initialPosition;
    private Transform player; 
    private bool returning = false;

    private void Start()
    {
        initialPosition = transform.position;
        FindPlayer();
    }

    private void Update()
    {
        if (!returning)
        {
            // Mover la punta de la lengua hacia adelante
            transform.Translate(Vector3.forward * tipSpeed * Time.deltaTime);
            if (Vector3.Distance(initialPosition, transform.position) >= maxDistance)
            {
                StartReturn();
            }
        }
        else
        {
            // Mover la punta de la lengua hacia el player
            transform.position = Vector3.MoveTowards(transform.position, player.position, returnSpeed * Time.deltaTime);
            if (Vector3.Distance(player.position, transform.position) < 0.1f && returning)
            if (Vector3.Distance(player.position, transform.position) < 0.1f && returning)
            {
                Destroy(gameObject);
            }
        }
    }
    private void LateUpdate()
    {
        lr.SetPosition(1, gameObject.transform.position);
        lr.SetPosition(0, player.position + transform.forward * 0.1f);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            EnemyScript enemyScript = other.gameObject.GetComponent<EnemyScript>();
            enemyScript.TakeDamage(attackValue);
        }
        if (!other.gameObject.CompareTag("Player") | IsTerrain(other.gameObject)) StartReturn();
    }

    private void StartReturn()
    {
        returning = true;
    }
    private void FindPlayer()
    {
        GameObject playerObject = GameObject.FindWithTag(playerTag);
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogWarning("Player not found initially.");
        }
    }
    private bool IsTerrain(GameObject obj)
    {
        return ((1 << obj.layer) & terrainLayer) != 0;
    }
}

