using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tounge : MonoBehaviour
{
    public Transform toungeBase, cam; //Transform de la base de la lengua en el player y de la camara
    public float toungeRange; //Rango de la lengua (por ahora no se usa)
    public float toungeDelayTime; // delay para que la lengua llegue e su objetivo (por ahora no se usa)
    public float fireRate; // ratio de disparo de la lengua
    public float fireTimer; // variable de conteo para el ratio de fuego.
    public GameObject toungePrefab; //prefab de la punta de la lengua
    private Transform toungeTip; //prefab de la punta de la lengua
    
    private void Start()
    {
        fireTimer = 0; //inicializar timer
    }

    private void Update()
    {
        //Iniciar ataque a rango
        if (Input.GetMouseButton(0) && fireTimer <= 0f)
        {
            fireTimer = 1 / fireRate;
            Attack();
        }
        //Contador de ratio de fuego
        if (fireTimer > 0) fireTimer -= Time.deltaTime;

        //Iniciar ataque Melee
        if (Input.GetMouseButton(1) && fireTimer <= 0f)
        {

        }

    }
   
    //funcion de ataque de rango
    public void Attack()
    {
        //iniciar el ataque
        GameObject bullet = Instantiate(toungePrefab, toungeBase.position, cam.rotation);
        //raycast para el ataque
        RaycastHit hit;
        if (Physics.Raycast(cam.position,cam.forward,out hit,10f))
        {
            bullet.transform.LookAt(hit.point);
        }
    }
}
