using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPUCarController : MonoBehaviour
{
    public Transform[] waypoints;
    public float maxSpeed = 10f;
    public float acceleration = 5f;
    public float rotationSpeed = 5f;
    public float waypointRadius = 1f;

    private int currentWaypointIndex = 0;

    private static bool raceStarted = false;
    private float currentSpeed = 0f;

    // Update is called once per frame
    void Update()
    {
        if (raceStarted)
        {
            if (waypoints.Length == 0) return;

            // Move towards the current waypoint
            Vector3 direction = waypoints[currentWaypointIndex].position - transform.position;
            direction.y = 0; // Ensure the car stays on the ground

            // Rotate the car to face the waypoint
            Quaternion toRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);

            // Gradually increase speed until reaching maxSpeed
            if (currentSpeed < maxSpeed)
            {
                currentSpeed += acceleration * Time.deltaTime;
            }

            // Move the car forward
            transform.position += transform.forward * currentSpeed * Time.deltaTime;

            // Check if the car is close enough to the waypoint
            if (direction.magnitude < waypointRadius)
            {
                // Move to the next waypoint
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            }
        }

    }


    public static void started()
    {
        raceStarted = true;
    }
}
