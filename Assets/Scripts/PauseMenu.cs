using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    // Boolean flags to track the state of the game and menu screens
    public static bool GameIsPaused = false;
    public static bool optionscreen = false;
    public static bool GameFinsished = false;

    // References to UI elements for the pause menu, options screen, and finish screen
    public GameObject pauseMenuUI;
    public GameObject OptionsScene;
    public GameObject FinishScene;

    // Reference to the text that displays the winner and time on the finish screen
    [SerializeField] TextMeshProUGUI screentext;

    // Slider for adjusting the master volume and reference to the audio mixer
    [SerializeField] Slider MasterVolume;
    [SerializeField] private AudioMixer myMixer;

    // Variables to store the winner's name and their finishing time
    public static string winner;
    public static string wintime;

    // Initialize the master volume slider at the start
    void Start()
    {
        MasterVolume.value = GetMasterLevel();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the Escape key is pressed to toggle pause/resume or options
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                if (optionscreen)
                {
                    back();  // Return from options to the pause menu
                }
                else
                {
                    Resume();  // Resume the game from pause
                }
            }
            else
            {
                Pause();  // Pause the game
            }
        }

        // If the game has finished, show the finish screen and display the results
        if (GameFinsished)
        {
            FinishScene.SetActive(true);
            screentext.text = winner + "\n" + wintime;  // Display the winner's name and time
        }
    }

    // Resume the game, hiding the pause menu and unfreezing the game
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;  // Resume the game's time progression
        GameIsPaused = false;
    }

    // Pause the game, showing the pause menu and freezing the game
    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;  // Freeze the game's time progression
        GameIsPaused = true;
    }

    // Load the options screen and hide the pause menu
    public void loadOptions()
    {
        OptionsScene.SetActive(true);
        pauseMenuUI.SetActive(false);
        optionscreen = true;
    }

    // Return from the options screen to the pause menu
    public void back()
    {
        OptionsScene.SetActive(false);
        pauseMenuUI.SetActive(true);
        optionscreen = false;
    }

    // Mark the game as finished and store the winner's name and finishing time
    public static void gameFinished(string text, string time)
    {
        GameFinsished = true;
        winner = text;
        wintime = time;
    }

    // Quit the game and load the main menu scene
    public void Quit()
    {
        SceneManager.LoadSceneAsync("Main Menu");
    }

    // Get the current master volume level from the audio mixer
    public float GetMasterLevel()
    {
        float value;
        bool result = myMixer.GetFloat("music", out value);

        // Return the volume level if successful, otherwise return 0
        return result ? value : 0f;
    }
}
