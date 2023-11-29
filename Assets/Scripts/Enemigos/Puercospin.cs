using UnityEngine;

namespace Enemigos
{
    public class Puercospin : EnemyScript
    {
        public int cantidadEspinas;
        public LayerMask whatIsPlayer;
        private Transform controllerUp;
        private Transform controllerMid;
        private Transform controllerDown;
        // Start is called before the first frame update
        private AudioSource audioSc;
        public AudioClip []attackSounds;
        private float audioAttackCD;
        public Animator anim;
        private void Start()
        {
            //SetStats(maxHealth, attackValue, moveSpeed, enemyLevel);
            controllerMid = this.gameObject.transform.GetChild(1);
            controllerUp = this.gameObject.transform.GetChild(2);
            controllerDown = this.gameObject.transform.GetChild(3);
            attackCooldown = Random.Range(4.5f, 5.5f);
            attackRange = 6f;
            audioSc = GetComponent<AudioSource>();
            audioAttackCD = 0.25f;
            anim = GetComponent<Animator>();
        }

        // Update is called once per frame
        private void Update()
        {
            CheckYValue();
            bool playerInRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

            switch (playerInRange)
            {
                case false:
                    Chase();
                    break;
                case true:
                    Attack();
                    if(audioAttackCD <= 0){
                        audioSc.PlayOneShot(attackSounds[Random.Range(0,attackSounds.Length)]);
                        audioAttackCD = 0.25f;
                    }
                    break;
            }

            CheckDistance();

            audioAttackCD -= Time.deltaTime;

        }
        // Emitter amount cantidad de emisores
        // 

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
        private void Attack() 
        {
            BS_Controller up = controllerUp.GetComponent<BS_Controller>();
            BS_Controller middle = controllerMid.GetComponent<BS_Controller>();
            BS_Controller down = controllerDown.GetComponent<BS_Controller>();

            up.emitterAmount = cantidadEspinas;
            middle.emitterAmount = cantidadEspinas;
            down.emitterAmount = cantidadEspinas;
            anim.SetTrigger("atk");
            
        }
    }
}
