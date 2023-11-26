using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Volume")]
    [SerializeField] private Slider volumeSlider;

    [Header("Game Start")]
    [SerializeField] private int tutorialScene;
    [SerializeField] private int gameMenuScene;


    public void StartGameScene()
    {
        if (PlayerPrefs.GetInt("TutorialCompleted")==1)
            SceneManager.LoadScene(gameMenuScene);
        else
            SceneManager.LoadScene(tutorialScene);
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("quitting");
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume/1000;
    }

    public void VolumeApply()
    {
        PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
    }
}
