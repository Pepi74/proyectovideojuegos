using UnityEngine;

namespace Enemigos
{
    public class Zorrillo : EnemyScript
    {
        public int cantidadEmitters;
        public float aguante = 20;
        public LayerMask whatIsPlayer;
        private Transform controllerUp;
        private Transform controllerMid;
        private Transform controllerDown;
        private bool attackStarted;
        // Start is called before the first frame update
        private void Start()
        {
            //SetStats(maxHealth, attackValue, moveSpeed, enemyLevel);
            controllerMid = this.gameObject.transform.GetChild(1);
            controllerUp = this.gameObject.transform.GetChild(2);
            controllerDown = this.gameObject.transform.GetChild(3);
            attackCooldown = Random.Range(2.5f, 3.5f);
            attackRange = 9f;
        }

        // Update is called once per frame
        private void Update()
        {
            bool playerInRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

            timeSinceLastAttack += Time.deltaTime;

            if (attackStarted) {
                if (timeSinceLastAttack >= aguante){
                    Chase();
                    timeSinceLastAttack = 0;
                } 
            } else
            {
                switch (playerInRange)
                {
                    case false:
                        Chase();
                        break;
                    case true:
                        Attack();
                        break;
                }
            }

            CheckDistance();


        }
        // Emitter amount cantidad de emisores
        // 

        private void Chase() 
        {
            Vector3 playerPosition = player.position;
            Vector3 moveDirection = (playerPosition - transform.position).normalized;
            transform.Translate(moveDirection * (moveSpeed * Time.deltaTime));        
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void Attack() 
        {
            BS_Controller up = controllerUp.GetComponent<BS_Controller>();
            BS_Controller middle = controllerMid.GetComponent<BS_Controller>();
            BS_Controller down = controllerDown.GetComponent<BS_Controller>();

            up.emitterAmount = cantidadEmitters;
            middle.emitterAmount = cantidadEmitters;
            down.emitterAmount = cantidadEmitters;
        
            attackStarted = true;

        }

    }
}
