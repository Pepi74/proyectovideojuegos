using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject interactionUI;

    private bool isPaused = false;
    private bool isGameOverMenuActive = false; // Flag to track game over menu state
    public CameraPosition cameraRotationScript;
    public PlayerMovement playerMovementScript;

    /* void Start()
    {
        Cursor.visible = false;
    } */

    void Update()
    {
        if(!isGameOverMenuActive)
        {
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

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        Cursor.visible = false;
        cameraRotationScript.SetCanRotate(true);
        playerMovementScript.SetCanMove(true);
        interactionUI.SetActive(true);
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        Cursor.visible = true;
        cameraRotationScript.SetCanRotate(false);
        playerMovementScript.SetCanMove(false);
        interactionUI.SetActive(false);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    // Method to set the game over menu state
    public void SetGameOverMenuState(bool isActive)
    {
        isGameOverMenuActive = isActive;
    }
}
