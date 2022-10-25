using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 0.3f;
    public Transform followCamera;

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        var mouseX = Mouse.current.delta.x.ReadValue() * mouseSensitivity;

        gameObject.transform.Rotate(Vector3.up * mouseX);
    }
}