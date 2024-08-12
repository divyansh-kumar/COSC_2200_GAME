using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Smoothing factors for the camera's position and rotation transitions
    public float positionSmoothing;
    public float rotationSmoothing;

    // Reference to the car's transform that the camera will follow
    public Transform carTransform;

    // Initialization logic (currently empty, but could be used for setup if needed)
    private void Start()
    {
        // You might want to set initial camera settings or configurations here if needed in the future
    }

    // Update is called once per frame
    private void Update()
    {
        // Smoothly move the camera towards the car's position
        transform.position = Vector3.Lerp(transform.position, carTransform.position, positionSmoothing);

        // Smoothly rotate the camera to match the car's rotation
        transform.rotation = Quaternion.Lerp(transform.rotation, carTransform.rotation, rotationSmoothing);

        // Lock the camera's X and Z rotation to keep it level with the ground
        transform.rotation = Quaternion.Euler(new Vector3(0, transform.rotation.eulerAngles.y, 0));
    }
}
