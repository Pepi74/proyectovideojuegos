using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI; // Menu de pausa
    public GameObject interactionUI; // UI de interaccion

    private bool isPaused = false; // Booleano que indica si el juego esta en pausa
    private bool isGameOverMenuActive = false; // Booleano que indica si el menu de game over esta activo
    public PlayerMovement playerMovementScript; // Referencia a script PlayerMovement del jugador

    void Update()
    {
        // Si el menu de gameover no esta activo, pasa esto
        if(!isGameOverMenuActive)
        {
            // Presionando "Esc" abre el menu de pausa
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }
    }

    // Reanudar juego
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        Cursor.visible = false;
        playerMovementScript.SetCanMove(true);
        interactionUI.SetActive(true);
    }

    // Pausar juego
    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        Cursor.visible = true;
        playerMovementScript.SetCanMove(false);
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
}
