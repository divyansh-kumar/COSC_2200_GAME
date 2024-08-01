using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public static bool optionscreen = false;
    public GameObject pauseMenuUI;
    public GameObject OptionsScene;
    public static bool GameFinsished = false;
    public GameObject FinishScene;
    [SerializeField] TextMeshProUGUI screentext;

    [SerializeField] Slider MasterVolume;
    [SerializeField] private AudioMixer myMixer;
    public static string winner;
    public static string wintime;


    void Start(){
        MasterVolume.value = GetMasterLevel();
     }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(GameIsPaused){
                if(optionscreen){
                    back();
                }else{
                    Resume();
                }
            }else{
                Pause();
            }
        }

        if (GameFinsished)
        {
            //Time.timeScale = 0f;
            FinishScene.SetActive(true);
            screentext.text = winner + "\n" + wintime;

        }
    }

    public void Resume(){
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause(){
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void loadOptions(){
        OptionsScene.SetActive(true);
        pauseMenuUI.SetActive(false);
        optionscreen = true;
    }

    public void back(){
        OptionsScene.SetActive(false);
        pauseMenuUI.SetActive(true);
        optionscreen = false;
    }

    public static void gameFinished(string text, string time)
    {
        GameFinsished = true;
        winner = text;
        wintime = time;
    }

    public void Quit(){
        SceneManager.LoadSceneAsync("Main Menu");
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
