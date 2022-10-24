using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    // Player inputs
    private PlayerInput cameraControls;

    // Player Actions
    private InputAction panAction;
    private InputAction rotateAction;
    private InputAction zoomAction;

    // Pan parameters
    Vector3 newPosition;
    public float movementSpeed;
    public float movementTime;

    // Rotation parameters
    Quaternion newRotation;
    public float rotationAmount;

    // Zoom parameters
    public Transform cameraTransform;
    Vector3 newZoom;
    public Vector3 zoomAmount;
    public float maxZoom;
    public float minZoom;

    private void Start()
    {
        newPosition = transform.position;
        
        newRotation = transform.rotation;

        newZoom = cameraTransform.localPosition;
        // Convert zoom figures to -ve
        maxZoom *= -1;
        minZoom *= -1;
    }

    private void Awake() {
        cameraControls = GetComponent<PlayerInput>();
        
        panAction = cameraControls.actions["Pan"];
        rotateAction = cameraControls.actions["Rotate"];
        zoomAction = cameraControls.actions["Zoom"];
    }

    private void OnEnable() {
        panAction.Enable();
        // cameraControls.actions["Pan"].performed += panAction;
        rotateAction.Enable();
        zoomAction.Enable();
    }
    // iv = in vector
    void onPan(Vector2 iv)
    {
        if (iv.x != 0)
        {
            newPosition += (transform.right * (iv.x * movementSpeed));
        }
        if (iv.y != 0)
        {
            newPosition += (transform.forward * (iv.y * movementSpeed));
        }
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
    }
    // ir = in rotator
    void onRotate(float ir)
    {
        // Q value turn left
        if (ir < 0)
        {
            newRotation *= Quaternion.Euler(Vector3.up * (ir * rotationAmount));
        }
        // E value turn right
        else if(ir > 0)
        {
            newRotation *= Quaternion.Euler(Vector3.up * (ir * rotationAmount));
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime);
    }
    // iz = in zoom
    void onZoom (float iz)
    {
        // Jank central
        if (iz > 0)
        {
            if (maxZoom < newZoom.z)
            {
                newZoom -= zoomAmount;
            }
        }
        else if(iz < 0)
        {
            if (minZoom > newZoom.z)
            {
                newZoom += zoomAmount;
            }
        }
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * movementTime);
    }

    private void FixedUpdate() {
        onPan(iv : panAction.ReadValue<Vector2>());
        onRotate(ir : rotateAction.ReadValue<float>());
        onZoom(iz : zoomAction.ReadValue<float>());
    }
}
