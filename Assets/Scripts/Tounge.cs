using UnityEngine;

public class Tounge : MonoBehaviour
{
    public bool canAttack = true; //Variable general que permite al jugador atacar

    public Transform toungeBase, cam; //Transform de la base de la lengua en el player y de la camara
    public float toungeRange; //Rango de la lengua (por ahora no se usa)
    public float toungeDelayTime; // delay para que la lengua llegue e su objetivo (por ahora no se usa)
    public float fireRate; // ratio de disparo de la lengua
    public float fireTimer; // variable de conteo para el ratio de fuego.
    public GameObject toungePrefab; //prefab de la punta de la lengua
    private Transform toungeTip; //prefab de la punta de la lengua
    
    public float meleeeRange; //Rango de ataque melee
    public int meleeDamage; // daño de melee
    public float meleeFireRate; // ratio de ataque melee
    public float meleeTimer; // variable de conteo para el ataque melee.
    private AudioSource audioSc;
    public AudioClip rangeAttackSound;
    public PlayerScript playerScript;

    private void Start()
    {
        audioSc = GetComponent<AudioSource>();
        fireTimer = 0; //inicializar timer
        if (Camera.main != null) cam = Camera.main.transform;
    }

    private void Update()
    {
        //Iniciar ataque a rango
        if (Input.GetMouseButton(0) && fireTimer <= 0f && canAttack && (int)playerScript.currentStamina > 0)
        {
            fireTimer = 1 / fireRate;
            meleeTimer = 0.3f;
            Attack();
            audioSc.PlayOneShot(rangeAttackSound);
            playerScript.StaminaChange(-10);
        }
        //Iniciar ataque Melee
        if (Input.GetMouseButton(1) && meleeTimer <= 0f && canAttack)
        {
            fireTimer = 0.3f;
            meleeTimer = 1 / meleeFireRate;
            MeleeAttack();
        }

        //Contador de ratio de fuego
        if (fireTimer > 0) fireTimer -= Time.deltaTime;

        

    }
   
    //funcion de ataque de rango
    public void Attack()
    {
        //iniciar el ataque
        GameObject bullet = Instantiate(toungePrefab, toungeBase.position, cam.rotation);
        //raycast para el ataque
        /*RaycastHit hit;
        if (Physics.Raycast(cam.position,cam.forward,out hit,10f))
        {
            bullet.transform.LookAt(hit.point);
        }*/
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void MeleeAttack()
    {
        foreach (Collider other in Physics.OverlapSphere(toungeBase.position, meleeeRange))
        {
            if (!other.gameObject.CompareTag("Enemy")) continue;
            EnemyScript enemyScript = other.gameObject.GetComponent<EnemyScript>();
            enemyScript.TakeDamage(meleeDamage);
        }
    }
}
