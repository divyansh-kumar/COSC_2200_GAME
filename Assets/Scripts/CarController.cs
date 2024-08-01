using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private float horizontalInput, verticalInput;
    private float currentSteerAngle, currentbreakForce;
    private bool isBreaking;
    private float breakvalue = 0f;
    private bool w_press, s_press;
    private static bool isGameRunning = false;

    [SerializeField] private float motorForce, breakForce, maxSteerAngle;
    [SerializeField] private float decelerationSpeed;
    [SerializeField] private float maxSpeed = 50f; // Maximum speed of the car
    [SerializeField] private float resetThreshold = -10f; // Y position threshold to reset the car
    [SerializeField] private Vector3 resetPosition; // Position to reset the car if it flips or goes out of bounds
    [SerializeField] private float resetDelay = 2f; // Delay before resetting the car
    [SerializeField] private float flipThreshold = 45f; // Angle threshold to detect flipping
    [SerializeField] private float vibrationReductionFactor = 0.1f; // Factor to reduce vibrations

    [SerializeField] private WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider, rearRightWheelCollider;

    [SerializeField] private Transform frontLeftWheelTransform, frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform, rearRightWheelTransform;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -0.5f, 0); // Lower the center of mass for stability
    }

    private void FixedUpdate()
    {
        if (isGameRunning)
        {
            GetInput();
            HandleMotor();
            HandleSteering();
            UpdateWheels();
            LimitCarSpeed();
            CheckForFlip();
        }
    }

    public static void gamestarted()
    {
        isGameRunning = true;
    }

    private void GetInput()
    {
        // Steering Input
        horizontalInput = Input.GetAxis("Horizontal");

        // Acceleration Input
        verticalInput = Input.GetAxis("Vertical");

        // Breaking Input
        isBreaking = Input.GetKey(KeyCode.Space);

        w_press = Input.GetKey(KeyCode.W);
        s_press = Input.GetKey(KeyCode.S);
    }

    private void HandleMotor()
    {
        // Apply motor torque based on vertical input
        if (verticalInput != 0)
        {
            frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
            frontRightWheelCollider.motorTorque = verticalInput * motorForce;
            // Ensure brake force is off when accelerating
            breakvalue = 0f;
        }
        else
        {
            // When no input, gradually reduce the motor torque
            frontLeftWheelCollider.motorTorque = Mathf.Lerp(frontLeftWheelCollider.motorTorque, 0, Time.deltaTime * decelerationSpeed);
            frontRightWheelCollider.motorTorque = Mathf.Lerp(frontRightWheelCollider.motorTorque, 0, Time.deltaTime * decelerationSpeed);

            // Apply brake force if no keys are pressed
            if (!w_press && !s_press)
            {
                breakvalue = breakForce;
            }
        }

        // Apply brake force
        if (isBreaking || (verticalInput == 0 && !w_press && !s_press))
        {
            currentbreakForce = breakForce;
        }
        else
        {
            currentbreakForce = 0f;
        }

        ApplyBreaking();
    }

    private void ApplyBreaking()
    {
        frontRightWheelCollider.brakeTorque = currentbreakForce;
        frontLeftWheelCollider.brakeTorque = currentbreakForce;
        rearLeftWheelCollider.brakeTorque = currentbreakForce;
        rearRightWheelCollider.brakeTorque = currentbreakForce;
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }

    private void LimitCarSpeed()
    {
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    private void CheckForFlip()
    {
        if (transform.eulerAngles.z > flipThreshold && transform.eulerAngles.z < 360 - flipThreshold)
        {
            StartCoroutine(ResetCarPosition());
        }

        if (transform.position.y < resetThreshold)
        {
            StartCoroutine(ResetCarPosition());
        }
    }

    private IEnumerator ResetCarPosition()
    {
        yield return new WaitForSeconds(resetDelay);
        transform.position = resetPosition;
        rb.velocity = Vector3.zero;
        transform.rotation = Quaternion.identity;
        print("Car reset to position");
    }
}
