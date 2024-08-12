using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    // References to the different UI scenes in the main menu
    public GameObject MainMenuUI;
    public GameObject OptionsScene;
    public GameObject LeaderScene;

    // Sliders for adjusting different audio volumes
    [SerializeField] Slider MasterVolume;
    [SerializeField] Slider MusicVolume;
    [SerializeField] Slider EffectsVolume;

    // Reference to the audio mixer for controlling sound levels
    [SerializeField] private AudioMixer myMixer;

    // Text component for displaying the leaderboard
    [SerializeField] public TextMeshProUGUI leadertext;

    // Start is called before the first frame update
    void Start()
    {
        // Initially show the main menu and hide the other scenes
        OptionsScene.SetActive(false);
        MainMenuUI.SetActive(true);
        LeaderScene.SetActive(false);

        // Set the master volume slider to the current master volume level
        MasterVolume.value = GetMasterLevel();
    }

    // Method to start the game and load the main playground scene
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("Playground");
    }

    // Method to display the options scene and hide other scenes
    public void Options()
    {
        OptionsScene.SetActive(true);
        MainMenuUI.SetActive(false);
        LeaderScene.SetActive(false);
    }

    // Method to display the leaderboard scene and hide other scenes
    public void Leaderboard()
    {
        OptionsScene.SetActive(false);
        MainMenuUI.SetActive(false);
        LeaderScene.SetActive(true);

        // Retrieve and display the win records from the database
        List<WinRecord> wins = DatabaseManager.GetWins();
        foreach (var win in wins)
        {
            // Append each win record to the leaderboard text
            leadertext.text += $"\nPlayer: {win.PlayerName}, Time: {win.Time}, Date: {win.Date}";
            print($"Player: {win.PlayerName}, Time: {win.Time}, Date: {win.Date}");
        }
    }

    // Method to quit the game (placeholder for now)
    public void QuitGame()
    {
        print("quit");  // Log a message indicating the game should quit
    }

    // Method to adjust the music volume based on the slider value
    public void SetMusicVolume()
    {
        float volume = MasterVolume.value;
        myMixer.SetFloat("music", volume);  // Set the music volume in the audio mixer
    }

    // Method to return to the main menu from other scenes
    public void back()
    {
        OptionsScene.SetActive(false);
        MainMenuUI.SetActive(true);
        LeaderScene.SetActive(false);
    }

    // Method to get the current master volume level from the audio mixer
    public float GetMasterLevel()
    {
        float value;
        bool result = myMixer.GetFloat("music", out value);

        // Return the volume level if successful, otherwise return 0
        return result ? value : 0f;
    }
}
