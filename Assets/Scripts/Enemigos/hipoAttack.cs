using UnityEngine;

namespace Enemigos
{
    public class HipoAttack : MonoBehaviour
    {
        public float timeActiveDamage = 2;
        public float lifeTime = 3;
        public float timeAlive;
        public int damage = 5;
        private PlayerScript playerScript;
        public AudioSource sonidoExplosion;
        // Start is called before the first frame update
        private void Start()
        {
            playerScript = GameObject.FindGameObjectWithTag("Player").transform.GetComponent<PlayerScript>();
            sonidoExplosion.pitch = Random.Range(0.8f,1.2f);
            sonidoExplosion.Play();
            Destroy(gameObject, lifeTime);
        }

        // Update is called once per frame
        private void Update()
        {
            timeAlive += Time.deltaTime;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Player") || !(timeActiveDamage > timeAlive)) return;
            playerScript.TakeDamage(damage);
        }
    }
}
