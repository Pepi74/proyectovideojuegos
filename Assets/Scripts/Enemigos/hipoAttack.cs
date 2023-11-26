using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hipoAttack : MonoBehaviour
{
    public float timeActiveDamage = 2;
    public float lifeTime = 3;
    public float timeAlive = 0;
    public int damage = 5;
    private PlayerScript playerScript;
    public AudioSource sonidoExplosion;
    // Start is called before the first frame update
    void Start()
    {
        PlayerScript playerScript = GameObject.FindGameObjectWithTag("Player").transform.GetComponent<PlayerScript>();
        sonidoExplosion.pitch = UnityEngine.Random.Range(0.8f,1.2f);
        sonidoExplosion.Play();
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        timeAlive += Time.deltaTime;
    }

    void OnTriggerEnter(Collider other){
        
        if(other.gameObject.tag=="Player" && timeActiveDamage > timeAlive)
        {
            PlayerScript playerScript = GameObject.FindGameObjectWithTag("Player").transform.GetComponent<PlayerScript>();
            playerScript.TakeDamage(damage);
        }
    }
}
