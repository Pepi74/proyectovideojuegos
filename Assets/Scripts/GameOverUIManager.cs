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

    public AudioSource audioSource;

    public Camera cam;
    public ThirdPersonCamera thirdPersonCamera;

    // Inicializacion de botones
    private void Start()
    {
        restartButton.onClick.AddListener(RestartGame);
        exitButton.onClick.AddListener(ExitGame);
        cam = Camera.main;
        if (cam != null) thirdPersonCamera = cam.GetComponent<ThirdPersonCamera>();
    }

    // Muestra la pantalla de game over
    public void ShowGameOverScreen()
    {
        gameOverScreen.SetActive(true);
        interactionUI.SetActive(false);
        Time.timeScale = 0f;
        pauseMenu.SetGameOverMenuState(true);
        roundText.color = Color.red;
        roundNumber = GameObject.Find("GameManager").GetComponent<GameManager>().roundNumber;
        if(roundNumber == 1) roundText.text = "You have survived " + roundNumber + " round!";
        else roundText.text = "You have survived " + roundNumber + " rounds!";
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        audioSource.Stop();
        thirdPersonCamera.SetCanMoveCamera(false);
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
        SceneManager.LoadScene(0); // Exit the game to main menu
    }
}
