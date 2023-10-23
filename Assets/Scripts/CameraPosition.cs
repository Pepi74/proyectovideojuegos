using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    public Vector3 playerPosition;
    public Vector3 cameraRotation;
    public Vector3 offset;

    private bool canRotate = true; // Add this variable
    void Start()
    {
        cameraRotation = new Vector3(30f,0f,0f);
        transform.eulerAngles = cameraRotation;
        offset = new Vector3(0,5,-7);
    }

    // Update is called once per frame
    void Update()
    {
        if(canRotate)
        {
            if(player != null)
            {
                playerPosition = player.transform.position;
                transform.position = playerPosition + player.transform.TransformDirection(offset);
                Vector3 rotation = new Vector3(0, player.transform.eulerAngles.y, 0) + cameraRotation;
                transform.eulerAngles = rotation;
            }
        }
    }

    public void SetCanRotate(bool rotate)
    {
        canRotate = rotate;
    }
}
