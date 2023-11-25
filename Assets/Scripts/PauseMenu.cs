using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI; // Menu de pausa
    public GameObject interactionUI; // UI de interaccion

    public bool isPaused; // Booleano que indica si el juego esta en pausa
    private bool isGameOverMenuActive; // Booleano que indica si el menu de game over esta activo
    public PlayerMovement playerMovementScript; // Referencia a script PlayerMovement del jugador
    public PlayerScript playerScript;
    //public GameObject playerCrosshair; // Por ahora el crosshair esta desactivado

    private void Awake()
    {
        Spawner spawner = FindObjectOfType<Spawner>();
        spawner.onPlayerSpawned.AddListener(SetPlayerReference);
    }

    private void Update()
    {
        // Si el menu de gameover no esta activo, pasa esto
        if (isGameOverMenuActive) return;
        // Presionando "Esc" abre el menu de pausa
        if (!Input.GetKeyDown(KeyCode.Escape) || playerScript.upgradeUIOpen) return;
        if (isPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    // Reanudar juego
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        playerMovementScript.SetCanMove(true);
        playerScript.SetCanRegen(true);
        //playerCrosshair.SetActive(true);
        interactionUI.SetActive(true);
    }

    // Pausar juego
    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        playerMovementScript.SetCanMove(false);
        playerScript.SetCanRegen(false);
        //playerCrosshair.SetActive(false);
        interactionUI.SetActive(false);
    }

    // Reiniciar juego
    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Salir del juego
    public void ExitGame()
    {
        Application.Quit();
    }

    // Metodo que setea el estado del menu game over
    public void SetGameOverMenuState(bool isActive)
    {
        isGameOverMenuActive = isActive;
    }

    private void SetPlayerReference(GameObject spawnedPlayer)
    {
        playerMovementScript = spawnedPlayer.GetComponent<PlayerMovement>();
        playerScript = spawnedPlayer.GetComponent<PlayerScript>();
        //playerCrosshair = spawnedPlayer.transform.Find("PlayerUI").gameObject.transform.Find("Crosshair").gameObject;
    }
}
