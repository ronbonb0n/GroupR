using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class Script_Player_Control : MonoBehaviour
{
    public Rigidbody Rb;
    public float Crouch_Speed = 600f;
    public float Move_Speed = 800f;
    public float Run_Speed = 1500f;
    public Player_Controls Player_Input;
    private InputAction Move;
    private InputAction Run;
    private InputAction Crouch;
    private Vector2 Move_Direction;
    public bool Is_Running = false;
    public bool Is_Crouching = false;
    public SphereCollider Collider;
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
            Collider.radius = 100f; 
        }
        else
            Collider.radius = 75f;
    }

    private void Crouch_Performed(InputAction.CallbackContext context)
    {
        Is_Crouching = !Is_Crouching;

        if (Is_Crouching)
        {
            Is_Running = false;
            Collider.radius = 50f;
        }
        else
            Collider.radius = 75f;
    }

    private void Update()
    {
       Move_Direction = Move.ReadValue<Vector2>();
       //Player_Vertical_Input = Input.GetAxis("Vertical");
       //Player_Horizontal_Input = Input.GetAxis("Horzizontal");

    }

    private void FixedUpdate()
    {

        /*Vector3 Forward = Camera.main.transform.forward;
        Vector3 Right = Camera.main.transform.right;
        Forward.y = 0;
        Right.y = 0;
        Forward = Forward.normalized;
        Right = Right.normalized;
        Vector3 Forward_Relative_Vertical_Input = Player_Vertical_Input * Forward;
        Vector3 Right_Relative_Vertical_Input = Player_Vertical_Input * Right;
        Vector3 Camera_Relative_Input = Forward_Relative_Vertical_Input + Right_Relative_Vertical_Input;
        this.transform.Translate(Camera_Relative_Input, Space.World);*/

        if (Is_Crouching)
            Rb.velocity = Crouch_Speed * Time.deltaTime * new Vector3(Move_Direction.y, 0, -Move_Direction.x);
        else if (Is_Running)
            Rb.velocity = Run_Speed * Time.deltaTime * new Vector3(Move_Direction.y, 0, -Move_Direction.x);
        else if (Is_Running == false && Is_Crouching == false)
            Rb.velocity = Move_Speed * Time.deltaTime * new Vector3(Move_Direction.y, 0, -Move_Direction.x);

    }
}