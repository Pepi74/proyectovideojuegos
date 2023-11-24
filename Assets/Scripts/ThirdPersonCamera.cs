using System;
using UnityEngine;
using Cinemachine;

public class ThirdPersonCamera : MonoBehaviour
{
    public GameObject player; // Referencia al jugador
    private CinemachineFreeLook thrdCamFreeLook;
    private CinemachineFreeLook combatCamFreeLook;

    public Transform orientation;

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

    private void Start()
    {
        rotationSpeed = 5f;
        SwitchCameraStyle(CameraStyle.Combat);
        // Bloquear el cursor en el centro
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Awake()
    {
        thrdCamFreeLook = thrdPersonCam.GetComponent<CinemachineFreeLook>();
        combatCamFreeLook = combatCam.GetComponent<CinemachineFreeLook>();
        Spawner spawner = FindObjectOfType<Spawner>();
        spawner.onPlayerSpawned.AddListener(SetPlayerReference);
    }

    private void Update()
    {
        if (player == null) return;
        var position = player.transform.position;
        var camPosition = transform.position;
        Vector3 viewDir = position - new Vector3(camPosition.x, position.y, camPosition.z);
        orientation.transform.forward = viewDir.normalized;

        // if (horizontalInput == 0 && verticalInput == 0)
        //     SwitchCameraStyle(CameraStyle.Basic);
        // else if (horizontalInput != 0 || verticalInput != 0)
        //     SwitchCameraStyle(CameraStyle.Combat);

        switch (currentStyle)
        {
            case CameraStyle.Basic:
            {
                float horizontalInput = Input.GetAxis("Horizontal");
                float verticalInput = Input.GetAxis("Vertical"); 
                Vector3 inputDir = orientation.transform.forward * verticalInput + orientation.right * horizontalInput;

                if (inputDir != Vector3.zero)
                {
                    player.transform.forward = Vector3.Slerp(player.transform.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
                }

                break;
            }
            case CameraStyle.Combat:
            {
                var combatLookAtPosition = combatLookAt.position;
                camPosition = transform.position;
                Vector3 dirToCombatLookAt = combatLookAtPosition - new Vector3(camPosition.x, combatLookAtPosition.y, camPosition.z);
                orientation.transform.forward = dirToCombatLookAt.normalized;

                player.transform.forward = dirToCombatLookAt.normalized;
                break;
            }
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void SetPlayerReference(GameObject spawnedPlayer)
    {
        player = spawnedPlayer;
        player.GetComponent<Rigidbody>();
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

        switch (newStyle)
        {
            case CameraStyle.Basic:
                thrdPersonCam.SetActive(true);
                break;
            case CameraStyle.Combat:
                combatCam.SetActive(true);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newStyle), newStyle, null);
        }

        currentStyle = newStyle;
    }
}
