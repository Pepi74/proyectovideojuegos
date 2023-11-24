using UnityEngine;

// Script sacado del video de Brackeys de la barra de vida
public class BillBoard : MonoBehaviour
{
    private Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
    }

    private void LateUpdate()
    {
        transform.LookAt(transform.position + mainCam.transform.forward);
    }
}
