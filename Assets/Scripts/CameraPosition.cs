using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    public GameObject player; // Referencia al jugador
    public Vector3 playerPosition; // Posicion del jugador
    public Vector3 cameraRotation; // Rotacion de camara
    public Vector3 offset; // Offset de posicion de camara

    // Inicalizacion de variables (se pueden modificar)
    void Start()
    {
        cameraRotation = new Vector3(30f,0f,0f);
        transform.eulerAngles = cameraRotation;
        offset = new Vector3(3,5,-7);
    }

    void Update()
    {
        // Si existe un jugador, cambia la posicion respecto al jugador
        if(player != null)
        {
            playerPosition = player.transform.position;
            transform.position = playerPosition + player.transform.TransformDirection(offset);
        }
    }
}
