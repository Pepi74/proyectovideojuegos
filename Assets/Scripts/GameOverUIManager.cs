using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverUIManager : MonoBehaviour
{
    public GameObject gameOverScreen;
    public GameObject interactionUI;
    public Button restartButton;
    public Button exitButton;

    public PauseMenu pauseMenu; // Reference to the PauseMenu script

    private void Start()
    {
        restartButton.onClick.AddListener(RestartGame);
        exitButton.onClick.AddListener(ExitGame);
    }

    public void ShowGameOverScreen()
    {
        gameOverScreen.SetActive(true);
        interactionUI.SetActive(false);
        Time.timeScale = 0f;
        pauseMenu.SetGameOverMenuState(true); // Set game over menu state
    }

    public void RestartGame()
    {
        // Restart the current scene
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitGame()
    {
        Application.Quit(); // Exit the game
    }
}
