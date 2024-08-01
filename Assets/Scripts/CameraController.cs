using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float positionSmoothing;
    public float rotationSmoothing;
    public Transform carTransform;

    private void Start()
    {

        
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, carTransform.position, positionSmoothing);
        transform.rotation = Quaternion.Lerp(transform.rotation, carTransform.rotation, rotationSmoothing);
        transform.rotation = Quaternion.Euler(new Vector3(0, transform.rotation.eulerAngles.y, 0));

    }
}
