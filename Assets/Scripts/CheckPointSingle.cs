using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointSingle : MonoBehaviour
{
    // References to player object, checkpoints, and other configurable parameters
    [SerializeField] GameObject player;
    [SerializeField] List<GameObject> checkPoints;
    [SerializeField] Vector3 vectorPoint;
    [SerializeField] float dead;
    [SerializeField] float maxSpeed = 100f; // Maximum speed of the car
    [SerializeField] Vector3 resetPosition; // Position to reset the car if it goes out of bounds
    [SerializeField] float resetThreshold = -10f; // Y position threshold to reset the car

    // Internal state and references
    private Rigidbody rb;
    private int currentCheckpointIndex = 0;

    // Initialize components and variables
    private void Start()
    {
        rb = player.GetComponent<Rigidbody>(); // Get the Rigidbody component attached to the player
    }

    // Update is called once per frame
    private void Update()
    {
        // If the player car's Y position goes below the reset threshold, reset its position
        if (player.transform.position.y < resetThreshold)
        {
            ResetCarPosition();
        }

        // Ensure the car's speed does not exceed the maximum limit
        LimitCarSpeed();
    }

    // Trigger detection for checkpoints
    private void OnTriggerEnter(Collider other)
    {
        // Check if the car has hit the correct checkpoint
        if (other.gameObject == checkPoints[currentCheckpointIndex])
        {
            vectorPoint = player.transform.position; // Store the player's position
            Destroy(other.gameObject); // Remove the checkpoint object
            print("Checkpoint completed");

            // Check if the player has reached the last checkpoint
            if (currentCheckpointIndex == checkPoints.Count - 1)
            {
                GameOver(); // Handle the end of the race
            }
            else
            {
                currentCheckpointIndex++; // Move to the next checkpoint
            }
        }
    }

    // Ensure the car's speed doesn't exceed the maximum speed limit
    private void LimitCarSpeed()
    {
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed; // Cap the car's velocity at the max speed
        }
    }

    // Reset the car's position and velocity if it goes out of bounds
    private void ResetCarPosition()
    {
        player.transform.position = resetPosition; // Move the car to the reset position
        rb.velocity = Vector3.zero; // Stop the car's movement
        print("Car reset to position");
    }

    // Handle the end of the race, stopping the timer and logging the win
    private void GameOver()
    {
        print("Race finished!");
        Timer.StopTimer(); // Stop the race timer
        PauseMenu.gameFinished("You won", Timer.getTime()); // Indicate the player has won
        DatabaseManager.LogWin("You", Timer.getTime()); // Log the win in the database
    }
}
