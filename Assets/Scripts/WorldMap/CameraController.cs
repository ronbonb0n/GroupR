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
    public Transform cameraTransform; // Required as we basically just move the camera in and out to zoom - explained below
    Vector3 newZoom;
    public Vector3 zoomAmount;
    public float maxZoom;
    public float minZoom;

    private void Start()
    {
        newPosition = transform.position;
        
        newRotation = transform.rotation;

        newZoom = cameraTransform.localPosition;
    }

    private void Awake() {
        cameraControls = GetComponent<PlayerInput>();
        
        panAction = cameraControls.actions["Pan"];
        rotateAction = cameraControls.actions["Rotate"];
        zoomAction = cameraControls.actions["Zoom"];
    }

    private void OnEnable() {
        panAction.Enable();
        // panAction.performed += onPan; - Rohan 
        rotateAction.Enable();
        // rotateAction.performed += onRotate; - Rohan
        zoomAction.Enable();
        // zoomAction.performed += onZoom; - Rohan
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
        // Jank central - this function only works at 45 degree camera rotation, can be fixed with basic trigonmetry though
        // Current camera position is -144 on Z axis, by zooming out we subtract from this value
        // Therefore zooming out requires a smaller value whilst zooming in requires a larger z value
        // It works - that's all i care about
        // Max and min zooms are public and defined within the scene
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

    // Camera controls currently on fixed update
    // Takes the value from the input and uses it to determine movement/rotation/zoom direction
    private void FixedUpdate() {
        onPan(iv : panAction.ReadValue<Vector2>());
        onRotate(ir : rotateAction.ReadValue<float>());
        onZoom(iz : zoomAction.ReadValue<float>());
    }
}
