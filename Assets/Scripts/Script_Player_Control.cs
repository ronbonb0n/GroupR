using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class Script_Player_Control : MonoBehaviour
{
    public Rigidbody Rb;
    public float Crouch_Speed = 800f;
    public float Move_Speed = 1500f;
    public float Run_Speed = 2200f;
    public float Speed;
    public Player_Controls Player_Input;
    private InputAction Move;
    private InputAction Run;
    private InputAction Crouch;
    private Vector2 Move_Direction;
    public bool Is_Running = false;
    public bool Is_Crouching = false;
    public SphereCollider Collider;
    public Transform Camera_Transform;
    //private float Player_Horizontal_Input; // WILL TEST IT LATER FOR MOVEMENT RELATIVE TO CAMERA
    //private float Player_Vertical_Input;




    private void Awake()
    {
        Player_Input = new Player_Controls();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
    }

    private void OnEnable()
    {
        Move = Player_Input.Player_Input.Movement;
        Move.Enable();
        Run = Player_Input.Player_Input.Run;
        Run.Enable();
        Run.performed += Run_Performed;
        Crouch = Player_Input.Player_Input.Crouch;
        Crouch.Enable();
        Crouch.performed += Crouch_Performed;
    }


    private void OnDisable()
    {
        Move.Disable();
        Run.Disable();
        Crouch.Disable();
    }

    private void Run_Performed(InputAction.CallbackContext context)
    {
        Is_Running = !Is_Running;

        if (Is_Running)
        { 
            Is_Crouching = false;
            Collider.radius = 3f; 
        }
        else
            Collider.radius = 2f;
    }

    private void Crouch_Performed(InputAction.CallbackContext context)
    {
        Is_Crouching = !Is_Crouching;

        if (Is_Crouching)
        {
            Is_Running = false;
            Collider.radius = 1f;
        }
        else
            Collider.radius = 2f;
    }

    private void Update()
    {
        Move_Direction = Move.ReadValue<Vector2>();
        Vector3 Rotation_Direction = gameObject.transform.position - new Vector3(Camera_Transform.position.x, gameObject.transform.position.y, Camera_Transform.position.z);
        gameObject.transform.forward = Rotation_Direction.normalized;
        
    }

    

    private void FixedUpdate()
    {

        Speed = Move_Speed;
        if (Is_Crouching) Speed = Crouch_Speed;
        else if (Is_Running) Speed = Run_Speed;
        Vector3 moveDirection = gameObject.transform.forward * Move_Direction.y + gameObject.transform.right * Move_Direction.x;
        Rb.AddForce(moveDirection * Speed * Time.deltaTime, ForceMode.Force);

       }
}