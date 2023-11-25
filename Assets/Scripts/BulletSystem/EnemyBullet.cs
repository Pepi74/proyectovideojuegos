using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum bulletType
{
    player,
    enemy
}
public class EnemyBullet : MonoBehaviour
{
    public float base_speed = 20f;
    public int Damage = 5;
    public float lifeTime = 10f;
    public bulletType shooter;

    private Renderer mesh; 

    // Componentes
    private Rigidbody RB;
    [SerializeField] private Vector3 direction = new Vector3(-1,0,0);

    // Especial
    public bool desviar;
    public Vector3 desviar_dir = new Vector3(1f,0,0);

    private float duration;

    void Start()
    {
        RB = GetComponent<Rigidbody>();
        Destroy(gameObject, lifeTime);
        mesh = GetComponent<Renderer>();
        duration = Time.time + 2f;
    }

    void OnBecameInvisible(){   // selfdestroy
        //mesh.enabled = false;
        //Destroy(gameObject,2f);
    }
    void OnBecameVisible(){
        //mesh.enabled = true;
    }
    void OnTriggerEnter(Collider other){
        
        if(other.gameObject.tag == "Player" && shooter == bulletType.enemy)
        {
            PlayerScript playerScript = other.gameObject.GetComponent<PlayerScript>();
            playerScript.TakeDamage(Damage);
            Destroy(gameObject);
        } /*else if (other.gameObject.tag == "Enemy" && shooter == bulletType.player)
        {
            Enemy cs = other.gameObject.GetComponent<Enemy>();
            cs.getDamage(Damage);
        }*/
        
        /*else if (other.gameObject.tag == "Terrain")
        {
            
        }
        else{
            Destroy(gameObject);
        }*/
    }

    public void UpdateVector(Vector3 new_vector){
        this.direction = new_vector;
    }

    void Update()
    {
        // Avanzar
        RB.velocity = transform.forward * base_speed;
        // Desviar
        if(desviar){
            transform.Rotate(desviar_dir * Time.deltaTime);
        }

        if (Time.time >= duration)
        {
            Destroy(this);
        }
    }
}
