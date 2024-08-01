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
    public GameObject MainMenuUI;
    public GameObject OptionsScene;
    public GameObject LeaderScene;

    [SerializeField] Slider MasterVolume;
    [SerializeField] Slider MusicVolume;
    [SerializeField] Slider EffectsVolume;

    [SerializeField] private AudioMixer myMixer;
    [SerializeField] public TextMeshProUGUI leadertext;

    void Start(){
        OptionsScene.SetActive(false);
        MainMenuUI.SetActive(true);
        LeaderScene.SetActive(false);
        MasterVolume.value = GetMasterLevel();


    }


    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("Playground");
    }

    public void Options()
    {
        OptionsScene.SetActive(true);
        MainMenuUI.SetActive(false);
        LeaderScene.SetActive(false);
    }

    public void Leaderboard()
    {
        OptionsScene.SetActive(false);
        MainMenuUI.SetActive(false);
        LeaderScene.SetActive(true);

        List<WinRecord> wins = DatabaseManager.GetWins();
        foreach (var win in wins)
        {
            leadertext.text = leadertext.text + "\n" + ($"Player: {win.PlayerName}, Time: {win.Time}, Date: {win.Date}");
            print($"Player: {win.PlayerName}, Time: {win.Time}, Date: {win.Date}");
        }

        
    }

    public void QuitGame()
    {
        print("quit");
    }


    public void SetMusicVolume(){
        float volume = MasterVolume.value;
        myMixer.SetFloat ("music", volume);


    }

    public void back(){
        OptionsScene.SetActive(false);
        MainMenuUI.SetActive(true);
        LeaderScene.SetActive(false);
    }

    public float GetMasterLevel(){
		float value;
		bool result =  myMixer.GetFloat("music", out value);
		if(result){
			return value;
		}else{
			return 0f;
		}
	}
}
