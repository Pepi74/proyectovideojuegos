using UnityEngine;

public class ToungeTip : MonoBehaviour
{
    public float tipSpeed = 10.0f;
    public float maxDistance = 10.0f;
    public float returnSpeed = 15.0f;
    public int attackValue;
    public LineRenderer lr;
    public LayerMask playerLayer;
    private int terrainLayerMask;
    private Vector3 initialPosition;
    public Transform player;
    private bool returning;
    public AudioSource audioSc;
    public AudioClip hitEnemySound;
    private void Start()
    {
        initialPosition = transform.position;
    }

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        audioSc = player.GetComponent<AudioSource>();
        attackValue = player.GetComponent<PlayerScript>().attackValue;
        terrainLayerMask = 1 << LayerMask.NameToLayer("Terrain");
    }

    private void Update()
    {
        if (!returning)
        {
            // Mover la punta de la lengua hacia adelante
            transform.Translate(Vector3.forward * (tipSpeed * Time.deltaTime));
            if (Vector3.Distance(initialPosition, transform.position) >= maxDistance)
            {
                StartReturn();
            }

            if (Physics.SphereCast(transform.position, 0.6f, transform.forward, out _, 0.1f, terrainLayerMask))
            {
                StartReturn();
            }
        }
        else
        {
            // Mover la punta de la lengua hacia el player
            transform.position = Vector3.MoveTowards(transform.position, player.position, returnSpeed * Time.deltaTime);
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
            audioSc.PlayOneShot(hitEnemySound);
        }
        if (other.gameObject.CompareTag("Boss"))
        {
            EnemyScript enemyScript = other.gameObject.GetComponent<EnemyScript>();
            enemyScript.TakeDamage(attackValue);
            audioSc.PlayOneShot(hitEnemySound);
        }
        if (!other.gameObject.CompareTag("Player")) StartReturn();
    }

    private void StartReturn()
    {
        returning = true;
    }
}

