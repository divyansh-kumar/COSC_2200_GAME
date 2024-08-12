using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPUCarController : MonoBehaviour
{
    // Array of waypoints that the CPU car will follow
    public Transform[] waypoints;

    // Car movement settings
    public float maxSpeed = 10f;        // Maximum speed the car can reach
    public float acceleration = 5f;     // How quickly the car accelerates
    public float rotationSpeed = 5f;    // How quickly the car rotates to face the waypoint
    public float waypointRadius = 1f;   // How close the car needs to get to a waypoint before moving to the next one

    // Index to keep track of the current waypoint
    private int currentWaypointIndex = 0;

    // Static variable to check if the race has started
    private static bool raceStarted = false;

    // Current speed of the car, starts at 0 and increases with acceleration
    private float currentSpeed = 0f;

    // Update is called once per frame
    void Update()
    {
        // Only move the car if the race has started
        if (raceStarted)
        {
            // If no waypoints are set, do nothing
            if (waypoints.Length == 0) return;

            // Determine the direction towards the current waypoint
            Vector3 direction = waypoints[currentWaypointIndex].position - transform.position;
            direction.y = 0; // Keep the car on the ground by ignoring vertical movement

            // Rotate the car to face the current waypoint
            Quaternion toRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);

            // Gradually increase speed until the car reaches its max speed
            if (currentSpeed < maxSpeed)
            {
                currentSpeed += acceleration * Time.deltaTime;
            }

            // Move the car forward based on its current speed
            transform.position += transform.forward * currentSpeed * Time.deltaTime;

            // Check if the car is close enough to the current waypoint
            if (direction.magnitude < waypointRadius)
            {
                // Move to the next waypoint, and loop back to the first if at the end
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            }
        }
    }

    // Method to start the race, allowing the car to begin moving
    public static void started()
    {
        raceStarted = true;
    }
}
