using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCamera_Controller : MonoBehaviour
{
    // Panning parameters
    Vector3 newPosition;
    float movementSpeed;
    public float fastSpeed;
    public float normalSpeed;
    public float movementTime;

    // Rotation parameters
    Quaternion newRotation;
    public float rotationAmount;

    // Zoom parameters
    public Transform cameraTransform; // Get camaera transform
    Vector3 newZoom;
    public Vector3 zoomAmount;
    public float maxZoom;
    public float minZoom;

    void Start()
    {
        newPosition = transform.position;
        newRotation = transform.rotation;
        newZoom = cameraTransform.localPosition;

        // Convert zoom figures to -ve
        // maxZoom *= -1;
        // minZoom *= -1;
        // Debug.Log(newZoom);
    }
    void Update()
    {
        HandleMovementInput();
    }

    void HandleMovementInput()
    {
        // Panning speed controls
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            movementSpeed = fastSpeed;
        }
        else
        {
            movementSpeed = normalSpeed;
        }
        // Panning direction controls
        if (Input.GetKey(KeyCode.W))
        {
            newPosition += (transform.forward * movementSpeed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            newPosition += (transform.forward * -movementSpeed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            newPosition += (transform.right * movementSpeed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            newPosition += (transform.right * -movementSpeed);
        }
        // Rotation controls
        if (Input.GetKey(KeyCode.Q))
        {
            newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
        }
        if (Input.GetKey(KeyCode.E))
        {
            newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
        }
        // Zoom controls
        if (Input.GetKey(KeyCode.R) || Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (minZoom > newZoom.z)
            {
                newZoom += zoomAmount;
            }
        }
        if (Input.GetKey(KeyCode.F) || Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (newZoom.z > maxZoom)
            {
                newZoom -= zoomAmount;
            }
        }
        // Output
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime);
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * movementTime);
    }
}
