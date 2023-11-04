using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script sacado del video de Brackeys de la barra de vida
public class BillBoard : MonoBehaviour
{
    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
    }

    void LateUpdate()
    {
        transform.LookAt(transform.position + mainCam.transform.forward);
    }
}
