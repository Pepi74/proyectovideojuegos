using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverUIManager : MonoBehaviour
{
    public GameObject gameOverScreen; // Menu game over
    public GameObject interactionUI; // UI interaccion
    public Button restartButton; // Boton reiniciar
    public Button exitButton; // Boton salir

    public PauseMenu pauseMenu; // Referencia al script PauseMenu del menu de pausa

    // Inicializacion de botones
    private void Start()
    {
        restartButton.onClick.AddListener(RestartGame);
        exitButton.onClick.AddListener(ExitGame);
    }

    // Muestra la pantalla de game over
    public void ShowGameOverScreen()
    {
        gameOverScreen.SetActive(true);
        interactionUI.SetActive(false);
        Time.timeScale = 0f;
        pauseMenu.SetGameOverMenuState(true);
    }

    // Metodo para reiniciar el juego
    private static void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Metodo para salir del juego
    private static void ExitGame()
    {
        Application.Quit(); // Exit the game
    }
}
