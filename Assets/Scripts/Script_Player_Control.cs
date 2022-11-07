using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class Script_Player_Control : MonoBehaviour
{
    public Rigidbody Rb;
    public float Crouch_Speed = 1000f;
    public float Move_Speed = 1400f;
    public float Run_Speed = 2000f;
    public Player_Controls Player_Input;
    private InputAction Move;
    private InputAction Run;
    private InputAction Crouch;
    private Vector2 Move_Direction;
    public bool Is_Running = false;
    public bool Is_Crouching = false;
    public SphereCollider Collider;
    private InputAction ability;
    private MeshRenderer character;
    //private SpriteRenderer character; USE IT AFTER SPRITES ARE ADDED
    //private Color col; USE IT FOR TRANSLUCENT COLOR
    private float activationTime;
    public bool invisble;

    void Start()
    {
        Rb = GetComponent<Rigidbody>();
        //character = GetComponent<SpriteRenderer>();
        character = GetComponent<MeshRenderer>();
        activationTime = 0;
        invisble = false;
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
        ability = Player_Input.Player_Input.Abilities;
        ability.Enable();
        ability.performed += Abitily_Performed;
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
        if (invisble == false)
        {
            invisble = true;
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

        if (Is_Running && invisble==false)
        { 
            Is_Crouching = false;
            Collider.radius = 100f; 
        }
        else if(Is_Running && invisble)
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
        else if (Is_Crouching && invisble == false)
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
        if (invisble && activationTime >= 3)
        {
            invisble = false;
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
        if (Is_Crouching)
            Rb.velocity = Crouch_Speed * Time.deltaTime * new Vector3(Move_Direction.y, 0, -Move_Direction.x);
        else if (Is_Running)
            Rb.velocity = Run_Speed * Time.deltaTime * new Vector3(Move_Direction.y, 0, -Move_Direction.x);
        else if (Is_Running == false && Is_Crouching == false)
            Rb.velocity = Move_Speed * Time.deltaTime * new Vector3(Move_Direction.y, 0, -Move_Direction.x);

    }
}