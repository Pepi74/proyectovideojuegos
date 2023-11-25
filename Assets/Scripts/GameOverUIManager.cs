using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverUIManager : MonoBehaviour
{
    public GameObject gameOverScreen; // Menu game over
    public GameObject interactionUI; // UI interaccion
    public Button restartButton; // Boton reiniciar
    public Button exitButton; // Boton salir
    public int roundNumber;
    public TextMeshProUGUI roundText;

    public PauseMenu pauseMenu; // Referencia al script PauseMenu del menu de pausa

    // Inicializacion de botones
    private void Start()
    {
        restartButton.onClick.AddListener(RestartGame);
        exitButton.onClick.AddListener(ExitGame);
        roundNumber = GameObject.Find("GameManager").GetComponent<GameManager>().roundNumber;
    }

    // Muestra la pantalla de game over
    public void ShowGameOverScreen()
    {
        gameOverScreen.SetActive(true);
        interactionUI.SetActive(false);
        Time.timeScale = 0f;
        pauseMenu.SetGameOverMenuState(true);
        roundText.color = Color.red;
        if(roundNumber == 1) roundText.text = "You have survived " + roundNumber + " round!";
        else roundText.text = "You have survived " + roundNumber + " rounds!";
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
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
