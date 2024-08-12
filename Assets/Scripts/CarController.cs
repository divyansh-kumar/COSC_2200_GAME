using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    // Input and control variables
    private float horizontalInput, verticalInput;
    private float currentSteerAngle, currentbreakForce;
    private bool isBreaking;
    private float breakvalue = 0f;
    private bool w_press, s_press;
    private static bool isGameRunning = false;

    // Car settings for driving and physics
    [SerializeField] private float motorForce, breakForce, maxSteerAngle;
    [SerializeField] private float decelerationSpeed;
    [SerializeField] private float maxSpeed = 50f; // Maximum speed the car can reach
    [SerializeField] private float resetThreshold = -10f; // If the car falls below this Y position, it will be reset
    [SerializeField] private Vector3 resetPosition; // Position where the car will be reset
    [SerializeField] private float resetDelay = 2f; // Delay before resetting the car after a flip or fall
    [SerializeField] private float flipThreshold = 45f; // Angle threshold to detect if the car is flipped
    [SerializeField] private float vibrationReductionFactor = 0.1f; // Factor to reduce minor vibrations

    // Wheel colliders and transforms for the car's wheels
    [SerializeField] private WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider, rearRightWheelCollider;
    [SerializeField] private Transform frontLeftWheelTransform, frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform, rearRightWheelTransform;

    // Rigidbody reference for controlling physics
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -0.5f, 0); // Lowering the center of mass for better stability during driving
    }

    private void FixedUpdate()
    {
        if (isGameRunning)
        {
            GetInput(); // Handle player inputs
            HandleMotor(); // Control acceleration and braking
            HandleSteering(); // Control steering based on input
            UpdateWheels(); // Sync wheel visuals with physics
            LimitCarSpeed(); // Enforce maximum speed
            CheckForFlip(); // Monitor if the car has flipped or fallen
        }
    }

    // Called when the game starts to allow the car to move
    public static void gamestarted()
    {
        isGameRunning = true;
    }

    // Gather input from the player for movement and actions
    private void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal"); // Steering
        verticalInput = Input.GetAxis("Vertical"); // Acceleration
        isBreaking = Input.GetKey(KeyCode.Space); // Braking
        w_press = Input.GetKey(KeyCode.W); // Forward input
        s_press = Input.GetKey(KeyCode.S); // Backward input
    }

    // Manage the car's motor and braking based on player input
    private void HandleMotor()
    {
        if (verticalInput != 0)
        {
            // Apply motor force to the front wheels based on input
            frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
            frontRightWheelCollider.motorTorque = verticalInput * motorForce;
            breakvalue = 0f; // No braking when accelerating
        }
        else
        {
            // Gradually decelerate if no input is given
            frontLeftWheelCollider.motorTorque = Mathf.Lerp(frontLeftWheelCollider.motorTorque, 0, Time.deltaTime * decelerationSpeed);
            frontRightWheelCollider.motorTorque = Mathf.Lerp(frontRightWheelCollider.motorTorque, 0, Time.deltaTime * decelerationSpeed);

            if (!w_press && !s_press)
            {
                breakvalue = breakForce; // Apply brake if no input
            }
        }

        // Apply braking if space is pressed or no input is given
        if (isBreaking || (verticalInput == 0 && !w_press && !s_press))
        {
            currentbreakForce = breakForce;
        }
        else
        {
            currentbreakForce = 0f; // No braking otherwise
        }

        ApplyBreaking(); // Apply the calculated brake force
    }

    // Apply brake force to all wheels
    private void ApplyBreaking()
    {
        frontRightWheelCollider.brakeTorque = currentbreakForce;
        frontLeftWheelCollider.brakeTorque = currentbreakForce;
        rearLeftWheelCollider.brakeTorque = currentbreakForce;
        rearRightWheelCollider.brakeTorque = currentbreakForce;
    }

    // Handle steering based on player input
    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput; // Calculate steer angle
        frontLeftWheelCollider.steerAngle = currentSteerAngle; // Apply to front left wheel
        frontRightWheelCollider.steerAngle = currentSteerAngle; // Apply to front right wheel
    }

    // Update the visual representation of the wheels to match their physical counterparts
    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    }

    // Sync a single wheel's position and rotation with its collider
    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot; // Set wheel rotation
        wheelTransform.position = pos; // Set wheel position
    }

    // Ensure the car does not exceed its maximum speed
    private void LimitCarSpeed()
    {
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed; // Cap speed to maxSpeed
        }
    }

    // Check if the car has flipped or fallen below the reset threshold
    private void CheckForFlip()
    {
        if (transform.eulerAngles.z > flipThreshold && transform.eulerAngles.z < 360 - flipThreshold)
        {
            StartCoroutine(ResetCarPosition()); // Start reset if car flips
        }

        if (transform.position.y < resetThreshold)
        {
            StartCoroutine(ResetCarPosition()); // Start reset if car falls too far
        }
    }

    // Reset the car's position and rotation after a delay
    private IEnumerator ResetCarPosition()
    {
        yield return new WaitForSeconds(resetDelay); // Wait before resetting
        transform.position = resetPosition; // Reset position
        rb.velocity = Vector3.zero; // Stop the car
        transform.rotation = Quaternion.identity; // Reset rotation
        print("Car reset to position");
    }
}
