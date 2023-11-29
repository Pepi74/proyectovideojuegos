using UnityEngine;

namespace Enemigos
{
    public class Pato : EnemyScript
    {
        private Rigidbody cuerpo;

        public float rangeStomp = 2;

        public LayerMask whatIsPlayer;

        private bool attackFinish = true;
        private bool attackPointSet;
        private Vector3 attackPoint;
        public ParticleSystem particulas;
        public ParticleSystem exclamationEffect;
        public TrailRenderer attackTrail;
        private AudioSource audioSc;
        public AudioClip []quackSounds;
        public AudioClip groundHitSound;
        private Animator anim;

        // Start is called before the first frame update
        private void Start()
        {
            cuerpo = GetComponent<Rigidbody>();
            player = GameObject.FindGameObjectWithTag("Player").transform; // Encuentra al jugador por el tag
            //SetStats(maxHealth, attackValue, moveSpeed, enemyLevel);
            attackCooldown = Random.Range(5.5f, 6.5f);
            attackRange = 2f;
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
            if (timeSinceLastAttack >= attackCooldown && (playerInRange || attackFinish)) Attack(); // Ataca
            CheckDistance();
        }

        private void Chase()
        {
            Vector3 playerPosition = player.position;
            var position = transform.position;
            Vector3 moveDirection = (playerPosition - position).normalized;
            AvoidObstacles(ref moveDirection);
            Transform transform1;
            (transform1 = transform).Translate(moveDirection * (moveSpeed * Time.deltaTime), Space.World);
            transform1.eulerAngles = new Vector3(0f, transform1.eulerAngles.y, 0f);
        }

        private void Attack()
        {   
            if (!attackPointSet) {
                attackPoint = player.position + new Vector3(0,10,0);
                attackPointSet = true;
                Instantiate(exclamationEffect, transform);
                Instantiate(attackTrail,transform);
                audioSc.PlayOneShot(quackSounds[Random.Range(0,quackSounds.Length)]);
            }
        
            if (attackPointSet) {
                cuerpo.useGravity = false; cuerpo.isKinematic = true;
                transform.position = Vector3.MoveTowards(transform.position,attackPoint,10 * Time.deltaTime);
            }

            Vector3 distanceToPlayer = transform.position - attackPoint;

            if (distanceToPlayer.magnitude < 3f){
                cuerpo.useGravity = true; cuerpo.isKinematic = false;
                cuerpo.AddForce(-transform.up * 80);
                attackPointSet = false;
                attackFinish = false;
            
                timeSinceLastAttack = 0.0f; // Resetea el timer
                anim.SetTrigger("Attack");
            }

        }

        private void OnCollisionEnter(Collision other)
        {
            PlayerScript playerScript = player.GetComponent<PlayerScript>();
            if (other.collider.CompareTag("Player")){
                playerScript.TakeDamage((int)(attackValue * 1.2f));
                attackFinish = true;

            } else if (other.collider.CompareTag("Terrain") && !attackFinish) {
                bool playerInRange = Physics.CheckSphere(transform.position, rangeStomp, whatIsPlayer);
                if (playerInRange) playerScript.TakeDamage(attackValue);
                attackFinish = true;
                Instantiate(particulas, transform);
                audioSc.PlayOneShot(groundHitSound,0.35f);
                cuerpo.velocity = Vector3.zero;
            }
        }

    }
}
