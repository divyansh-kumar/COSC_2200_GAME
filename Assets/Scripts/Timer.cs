using System;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    static float elapsedTime;
    private static bool isTimerRunning = false;
    public static string currenttime = "";

    private void Update()
    {
        if (isTimerRunning)
        {
            elapsedTime += Time.deltaTime;
            int min = Mathf.FloorToInt(elapsedTime / 60);
            int sec = Mathf.FloorToInt(elapsedTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", min, sec); 
            currenttime = string.Format("{0:00}:{1:00}", min, sec);
        }
    }

    public static void StartTimer()
    {
        isTimerRunning = true;
        elapsedTime = 0f; // Reset the timer if needed
    }

    public static void StopTimer()
    {
        isTimerRunning = false;

    }

    public static string getTime()
    {
        return currenttime;
    }
}
