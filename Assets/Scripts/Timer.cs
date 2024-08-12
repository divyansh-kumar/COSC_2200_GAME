using System;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    // Reference to the TextMeshProUGUI component that displays the timer
    [SerializeField] TextMeshProUGUI timerText;

    // Tracks the elapsed time since the timer started
    static float elapsedTime;

    // Indicates whether the timer is currently running
    private static bool isTimerRunning = false;

    // Stores the current time in a string format
    public static string currenttime = "";

    // Update is called once per frame
    private void Update()
    {
        // If the timer is running, update the elapsed time and display it
        if (isTimerRunning)
        {
            // Increase elapsed time by the time that has passed since the last frame
            elapsedTime += Time.deltaTime;

            // Calculate minutes and seconds from the elapsed time
            int min = Mathf.FloorToInt(elapsedTime / 60);
            int sec = Mathf.FloorToInt(elapsedTime % 60);

            // Update the timer text on the screen
            timerText.text = string.Format("{0:00}:{1:00}", min, sec);

            // Store the current time as a string for later use
            currenttime = string.Format("{0:00}:{1:00}", min, sec);
        }
    }

    // Starts the timer, resetting the elapsed time if necessary
    public static void StartTimer()
    {
        isTimerRunning = true;
        elapsedTime = 0f;  // Reset the timer to start from zero
    }

    // Stops the timer, freezing the elapsed time
    public static void StopTimer()
    {
        isTimerRunning = false;
    }

    // Returns the current time as a string in the format "MM:SS"
    public static string getTime()
    {
        return currenttime;
    }
}
