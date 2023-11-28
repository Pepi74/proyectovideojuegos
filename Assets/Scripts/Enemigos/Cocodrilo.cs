using UnityEngine;

namespace Enemigos
{
    public class Cocodrilo : EnemyScript
    {
        private Rigidbody cuerpo;
        public float impulseForce = 80;

        public LayerMask whatIsPlayer;

        private bool attackFinish = true;
        private Vector3 attackPoint;
        public ParticleSystem exclamationEffect;
        public TrailRenderer trailEffect;
        private AudioSource audioSc;
        public AudioClip []attackSounds;

        public Animator anim;
        // Start is called before the first frame update
        private void Start()
        {
            
            cuerpo = GetComponent<Rigidbody>();
            player = GameObject.FindGameObjectWithTag("Player").transform; // Encuentra al jugador por el tag		
            //SetStats(maxHealth, attackValue, moveSpeed, enemyLevel);
            attackCooldown = Random.Range(4.5f,5.5f);
            attackRange = 5f;
            audioSc = GetComponent<AudioSource>();
            anim = GetComponent<Animator>();
        }

        // Update is called once per frame
        private void Update()
        {
            CheckYValue();
            // Manejo enfriamiento del ataque
            timeSinceLastAttack += Time.deltaTime;

            bool playerInRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

            if (!playerInRange) Chase();
            if (timeSinceLastAttack >= attackCooldown && (playerInRange && attackFinish)) {
                Instantiate(exclamationEffect,transform);
                Instantiate(trailEffect,transform);
                audioSc.PlayOneShot(attackSounds[Random.Range(0,attackSounds.Length)]);
                Attack(); // Ataca
            }
            CheckDistance();
        }

        private void Chase()
        {
            Vector3 playerPosition = player.position;
            var position = transform.position;
            Vector3 moveDirection = (playerPosition - position).normalized;
            Transform transform1;
            (transform1 = transform).Translate(moveDirection * (moveSpeed * Time.deltaTime), Space.World);
            transform1.eulerAngles = new Vector3(0f, transform1.eulerAngles.y, 0f);
            transform.LookAt(player);
            attackFinish = true;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void Attack()
        {   
            cuerpo.AddForce((player.position - transform.position + new Vector3(0,1,0)).normalized * impulseForce, ForceMode.Impulse);
            anim.SetTrigger("atk");
            timeSinceLastAttack = 0f;
            attackFinish = false;

        }

        private void OnCollisionEnter(Collision other)
        {
            PlayerScript playerScript = player.GetComponent<PlayerScript>();
            if (!other.collider.CompareTag("Player")) return;
            playerScript.TakeDamage(attackValue);
            attackFinish = true;
        }

    }
}
