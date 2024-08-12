using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class MainTimer : MonoBehaviour
{
    // Reference to the UI text element that displays the countdown timer
    [SerializeField] TextMeshProUGUI timerText;

    // Time remaining before the race starts
    public float timeRemaining = 3;

    // Flag to ensure the race only starts once
    private bool timerstarted = false;

    // Initialize the timer text when the script starts
    private void Start()
    {
        timerText.text = "4"; // Display the initial countdown value
    }

    // Update is called once per frame
    void Update()
    {
        // Countdown logic: reduce the remaining time and update the displayed timer text
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime; // Decrease the time remaining
            timerText.text = string.Format("{0:0}", timeRemaining); // Display the remaining time as an integer
        }
        else
        {
            timerText.text = ""; // Clear the timer text when the countdown reaches zero

            // Start the race if it hasn't started yet
            if (timerstarted == false)
            {
                Timer.StartTimer(); // Start the main race timer
                CarController.gamestarted(); // Signal that the player car can start
                CPUCarController.started(); // Signal that the CPU-controlled car can start
                timerstarted = true; // Prevent the race from starting multiple times
            }
        }
    }
}
