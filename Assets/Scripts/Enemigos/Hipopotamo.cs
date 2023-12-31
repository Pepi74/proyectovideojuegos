using System.Collections;
using UnityEngine;

namespace Enemigos
{
    public class Hipopotamo : EnemyScript
    {
        private Rigidbody cuerpo;
        public float impulseForce = 80;

        public LayerMask whatIsPlayer;
        public LayerMask whatIsEnemy;
        public HipoAttack ataqueDistancia;
        private Vector3 attackPoint;

        private Animator anim;
        private static readonly int Attack1 = Animator.StringToHash("Attack1");
        private static readonly int Attack2 = Animator.StringToHash("Attack2");
        private static readonly int Attack3 = Animator.StringToHash("Attack3");

        // Start is called before the first frame update
        private void Start()
        {
            cuerpo = GetComponent<Rigidbody>();
            player = GameObject.FindGameObjectWithTag("Player").transform; // Encuentra al jugador por el tag
            //SetStats(maxHealth, attackValue, moveSpeed);
            SetStats(maxHealth, attackValue, moveSpeed, enemyLevel);
            anim = GetComponent<Animator>();
        }

        // Update is called once per frame
        private void Update()
        {
            CheckYValue();
            // Manejo enfriamiento del ataque
            timeSinceLastAttack += Time.deltaTime;

            var position = transform.position;
            bool playerInRange = Physics.CheckSphere(position, attackRange, whatIsPlayer);
            bool playerInRangeOfMeleeAttack = Physics.CheckSphere(position, attackRange * 0.9f, whatIsPlayer);
            bool playerInRangeOfRangedAttack = Physics.CheckSphere(position, attackRange + 5, whatIsPlayer);
            bool enemyInRange = Physics.CheckSphere(position, attackRange, whatIsEnemy);

            switch (playerInRange)
            {
                case false when enemyInRange && timeSinceLastAttack > attackCooldown:
                {
                    anim.SetTrigger(Attack3);// lanza otro enemigo al jugador
                    break;
                }
                case false when playerInRangeOfRangedAttack && timeSinceLastAttack > attackCooldown * 0.975f:
                    RangedAttack();
                    break;
                default:
                {
                    if (playerInRangeOfMeleeAttack && timeSinceLastAttack >= attackCooldown *0.9f)
                    {
                        //MeleeAttack(); // Ataca
                        anim.SetTrigger(Attack1);
                    }           
                    else if (!playerInRange) Chase(); // persigue al jugador

                    break;
                }
            }
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

        // ReSharper disable Unity.PerformanceAnalysis
        public void ThrowTeammate()
        {
            GameObject teammate = FindClosest();

            var position = transform.position;
            teammate.transform.position = position + new Vector3(0,4,0);
            teammate.GetComponent<Rigidbody>().AddForce((player.position - position).normalized * impulseForce * 0.95f, ForceMode.Impulse);

            timeSinceLastAttack = 0f;
        }

        private GameObject FindClosest()
        {
            float distanceToClosestFruit = Mathf.Infinity;
            GameObject closestFruit = null;
            GameObject[] allFruit = GameObject.FindGameObjectsWithTag("Enemy");

            foreach (GameObject currentFruit in allFruit) {
                float distanceToFruit = (currentFruit.transform.position - this.transform.position).sqrMagnitude;
                if (!(distanceToFruit < distanceToClosestFruit)) continue;
                distanceToClosestFruit = distanceToFruit;
                closestFruit = currentFruit;
            }

            return closestFruit;

        }

        public void MeleeAttack()
        {   
            cuerpo.AddForce((player.position - transform.position + new Vector3(0,2,0)).normalized * impulseForce, ForceMode.Impulse);
            timeSinceLastAttack = 0f;
        }

        private void RangedAttack()
        {
            StartCoroutine(RangeProyectiles());
            timeSinceLastAttack = 0f;
        }

        private IEnumerator RangeProyectiles()
        {
            var position = transform.position;
            Vector3 directionAttack = (player.position - position).normalized;
            anim.SetTrigger(Attack2);
            yield return new WaitForSeconds(0.4f);
            Instantiate(ataqueDistancia, (position + directionAttack*4), Quaternion.identity);
            yield return new WaitForSeconds(0.4f);
            Instantiate(ataqueDistancia, (position + directionAttack*8), Quaternion.identity);
            yield return new WaitForSeconds(0.4f);
            Instantiate(ataqueDistancia, (position + directionAttack*12), Quaternion.identity);
        }

        private void OnCollisionEnter(Collision other)
        {
            PlayerScript playerScript = player.GetComponent<PlayerScript>();
            if (other.collider.CompareTag("Player")){
                playerScript.TakeDamage(attackValue);
            }
        }



    }
}
