using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToungeTip : MonoBehaviour
{
    public float tipSpeed = 10.0f;
    public float maxDistance = 10.0f;
    public float returnSpeed = 15.0f;
    public  int attackValue;
    public Transform player;
    public LineRenderer lr;
    public LayerMask playerLayer;
    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private bool returning = false;

    private void Start()
    {
        initialPosition = transform.position;
        targetPosition = initialPosition + transform.forward * maxDistance;
        Collider[] colliders = Physics.OverlapSphere(initialPosition, maxDistance, playerLayer);
        if (colliders.Length > 0)
        {
            player = colliders[0].transform;
        }

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
        if (!other.gameObject.CompareTag("Player") | other.gameObject.CompareTag("Terrain")) StartReturn();
    }

    private void StartReturn()
    {
        returning = true;
    }
}

