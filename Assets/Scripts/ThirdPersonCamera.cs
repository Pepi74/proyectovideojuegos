using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ThirdPersonCamera : MonoBehaviour
{
    public GameObject player; // Referencia al jugador
    private CinemachineFreeLook thrdCamFreeLook;
    private CinemachineFreeLook combatCamFreeLook;

    public Transform orientation;
    public Rigidbody rb;

    public float rotationSpeed;

    public Transform combatLookAt;

    public GameObject thrdPersonCam;
    public GameObject combatCam;

    public CameraStyle currentStyle;

    public enum CameraStyle
    {
        Basic,
        Combat
    }

    void Start()
    {
        rotationSpeed = 5f;
        SwitchCameraStyle(CameraStyle.Combat);
        // Bloquear el cursor en el centro
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Awake()
    {
        thrdCamFreeLook = thrdPersonCam.GetComponent<CinemachineFreeLook>();
        combatCamFreeLook = combatCam.GetComponent<CinemachineFreeLook>();
        Spawner spawner = FindObjectOfType<Spawner>();
        spawner.onPlayerSpawned.AddListener(SetPlayerReference);
    }

    void Update()
    {

        if (player != null)
        {
            Vector3 viewDir = player.transform.position - new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
            orientation.transform.forward = viewDir.normalized;

            // if (horizontalInput == 0 && verticalInput == 0)
            //     SwitchCameraStyle(CameraStyle.Basic);
            // else if (horizontalInput != 0 || verticalInput != 0)
            //     SwitchCameraStyle(CameraStyle.Combat);

            if (currentStyle == CameraStyle.Basic)
            {       

                float horizontalInput = Input.GetAxis("Horizontal");
                float verticalInput = Input.GetAxis("Vertical"); 
                Vector3 inputDir = orientation.transform.forward * verticalInput + orientation.right * horizontalInput;

                if (inputDir != Vector3.zero)
                {
                    player.transform.forward = Vector3.Slerp(player.transform.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
                } 
            }

            else if (currentStyle == CameraStyle.Combat)
            {
                Vector3 dirToCombatLookAt = combatLookAt.position - new Vector3(transform.position.x, combatLookAt.position.y, transform.position.z);
                orientation.transform.forward = dirToCombatLookAt.normalized;

                player.transform.forward = dirToCombatLookAt.normalized;
            }
            
        }
    }

    void SetPlayerReference(GameObject spawnedPlayer)
    {
        player = spawnedPlayer;
        rb = player.GetComponent<Rigidbody>();
        orientation = player.transform.Find("Orientation").gameObject.transform;
        combatLookAt = player.transform.Find("Orientation").gameObject.transform.Find("CombatLookAt").gameObject.transform;

        thrdCamFreeLook.LookAt = player.transform;
        thrdCamFreeLook.Follow = player.transform;

        combatCamFreeLook.LookAt = combatLookAt;
        combatCamFreeLook.Follow = player.transform;        
    }

    private void SwitchCameraStyle(CameraStyle newStyle)
    {
        combatCam.SetActive(false);
        thrdPersonCam.SetActive(false);

        if(newStyle == CameraStyle.Basic) thrdPersonCam.SetActive(true);
        if(newStyle == CameraStyle.Combat) combatCam.SetActive(true);

        currentStyle = newStyle;
    }
}
