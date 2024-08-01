using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointSingle : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] List<GameObject> checkPoints;
    [SerializeField] Vector3 vectorPoint;
    [SerializeField] float dead;
    [SerializeField] float maxSpeed = 100f; // Maximum speed of the car
    [SerializeField] Vector3 resetPosition; // Position to reset the car if it goes out of bounds
    [SerializeField] float resetThreshold = -10f; // Y position threshold to reset the car

    private Rigidbody rb;
    private int currentCheckpointIndex = 0;

    private void Start()
    {
        rb = player.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Reset the car's position if it goes below the reset threshold
        if (player.transform.position.y < resetThreshold)
        {
            ResetCarPosition();
        }

        // Limit the car's speed
        LimitCarSpeed();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == checkPoints[currentCheckpointIndex])
        {
            vectorPoint = player.transform.position;
            Destroy(other.gameObject);
            print("Checkpoint completed");

            // Check if the player has reached the last checkpoint
            if (currentCheckpointIndex == checkPoints.Count - 1)
            {
                GameOver();
            }
            else
            {
                currentCheckpointIndex++;
            }
        }
    }

    private void LimitCarSpeed()
    {
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    private void ResetCarPosition()
    {
        player.transform.position = resetPosition;
        rb.velocity = Vector3.zero; // Reset velocity to stop the car
        print("Car reset to position");
    }

    private void GameOver()
    {
        
        print("Race finished!");
        Timer.StopTimer();
        PauseMenu.gameFinished("You won", Timer.getTime());
        DatabaseManager.LogWin("You", Timer.getTime());
    }
}
