using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: Cambiarlo a Cinemachine para mejor comportamiento de camara, mientras se usa este

public class CameraPosition : MonoBehaviour
{
    public GameObject player; // Referencia al jugador
    public Vector3 playerPosition; // Posicion del jugador
    public Vector3 cameraRotation; // Rotacion de camara
    public Vector3 offset; // Offset de posicion de camara
    public bool canRotate = true;

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
        if(player != null && canRotate)
        {
            playerPosition = player.transform.position;
            transform.position = playerPosition + player.transform.TransformDirection(offset);
            Vector3 rotation = new Vector3(player.transform.eulerAngles.x, player.transform.eulerAngles.y, 0) + cameraRotation;
            transform.eulerAngles = rotation;
        }
    }

    public void SetCanRotate(bool rotate)
    {
        canRotate = rotate;
    }
}
