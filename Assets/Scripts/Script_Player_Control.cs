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



    private InputAction ability;
    private MeshRenderer character;
    //private SpriteRenderer character; USE IT AFTER SPRITES ARE ADDED
    //private Color col; USE IT FOR TRANSLUCENT COLOR
    private float activationTime;
    public bool invisible;
    public Transform cameraTransform;

    void Start()
    {
        Rb = GetComponent<Rigidbody>();
        //character = GetComponent<SpriteRenderer>();
        character = GetComponent<MeshRenderer>();
        activationTime = 0;
        invisible = false;
        //col = character.material.color;

    }

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
        ability.Disable();
    }

    private void Abitily_Performed(InputAction.CallbackContext context)
    {
        if (invisible == false)
        {
            invisible = true;
            activationTime = 0;
            //col.a = 0.2f;
            //character.material.color = col;
            character.enabled = false;
            Collider.radius = 0f;
        }
    }

    private void Run_Performed(InputAction.CallbackContext context)
    {
        Is_Running = !Is_Running;

        if (Is_Running && invisible==false)
        { 
            Is_Crouching = false;
            Collider.radius = 100f; 
        }
        else if(Is_Running && invisible)
        {
            Is_Crouching = false;
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
        else if (Is_Crouching && invisible == false)
        {
            Is_Running = false;
        }
        else
            Collider.radius = 75f;
    }

    private void Update()
    {
        Move_Direction = Move.ReadValue<Vector2>();
        activationTime += Time.deltaTime;
        if (invisible && activationTime >= 3)
        {
            invisible = false;
            //col.a = 1f;
            //character.material.color = col;
            character.enabled = true;

            if (Is_Running)
            {
                Collider.radius = 100f;
            }
            else if (Is_Crouching)
            {
                Collider.radius = 50f;
            }
            else
            {
                Collider.radius = 75f;
            }
        }
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

        Vector3 moveDirection = gameObject.transform.forward * Move_Direction.y + gameObject.transform.right * Move_Direction.x;
        if (Is_Crouching)
            Rb.AddForce(moveDirection * Crouch_Speed * Time.deltaTime, ForceMode.Force);
        else if (Is_Running)
            Rb.AddForce(moveDirection * Run_Speed * Time.deltaTime, ForceMode.Force);
        else if (Is_Running == false && Is_Crouching == false)
            Rb.AddForce(moveDirection * Move_Speed * Time.deltaTime, ForceMode.Force);
        Quaternion rotationPlayer = new Quaternion(gameObject.transform.rotation.x, 0 , 0, gameObject.transform.rotation.w);
        transform.rotation = Quaternion.Lerp(rotationPlayer, cameraTransform.transform.rotation, 10 * Time.deltaTime);
    }
}