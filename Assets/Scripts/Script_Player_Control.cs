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
    public float Rotation_Speed;
    public Player_Controls Player_Input;
    private InputAction Move;
    private InputAction Run;
    private InputAction Crouch;
    private Vector2 Move_Direction;
    public bool Is_Running = false;
    public bool Is_Crouching = false;
    public SphereCollider Collider;
    private InputAction Cloak;
    private MeshRenderer Character;
    //private SpriteRenderer character; USE IT AFTER SPRITES ARE ADDED
    //private Color col; USE IT FOR TRANSLUCENT COLOR
    private float Activation_Time;
    public bool Invisble;
    public float Cloak_Count = 3.0f;
    public Transform frontObj;
    private InputAction EMP;
    private InputAction Decoy;
    public float Throw_Force = 40f;
    public GameObject EMP_Prefab;
    public GameObject Decoy_Prefab;
    public float EMP_Count = 3.0f;
    public float Decoy_Count = 3.0f;

    void Start()
    {
        Rb = GetComponent<Rigidbody>();
        //character = GetComponent<SpriteRenderer>();
        Character = GetComponent<MeshRenderer>();
        Activation_Time = 0;
        Invisble = false;
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
        Cloak = Player_Input.Player_Input.Cloak;
        Cloak.Enable();
        Cloak.performed += Cloak_Performed;
        EMP = Player_Input.Player_Input.EMP;
        EMP.Enable();
        EMP.performed += EMP_Performed;
        Decoy = Player_Input.Player_Input.Decoy;
        Decoy.Enable();
        Decoy.performed += Decoy_Performed;
    }


    private void OnDisable()
    {
        Move.Disable();
        Run.Disable();
        Crouch.Disable();
        Cloak.Disable();
        EMP.Disable();
        Decoy.Disable();
    }

    private void EMP_Performed(InputAction.CallbackContext context)
    {
        if (InventoryManager.instance.getItemCount("EMP") >= 1)
        {
            //Decrease EMP here?
            
            GameObject EMP = Instantiate(EMP_Prefab, transform.position, transform.rotation);
            Rigidbody RB = EMP.GetComponent<Rigidbody>();
            RB.AddForce(transform.forward * Throw_Force, ForceMode.VelocityChange);
            InventoryManager.instance.itemDecrement("EMP");
        }
    }

    private void Decoy_Performed(InputAction.CallbackContext context)
    {
        if (InventoryManager.instance.getItemCount("DECOY") >= 1)
        {
            //Decrease Decoy here?
            GameObject Decoy = Instantiate(Decoy_Prefab, transform.position, transform.rotation);
            Rigidbody RB = Decoy.GetComponent<Rigidbody>();
            RB.AddForce(transform.forward * Throw_Force, ForceMode.VelocityChange);
            InventoryManager.instance.itemDecrement("DECOY");
        }
    }

    private void Cloak_Performed(InputAction.CallbackContext context)
    {
        if (Invisble == false && Cloak_Count >= 1)
        {
            //Decrease EMP here?
            Invisble = true;
            Activation_Time = 0;
            //col.a = 0.2f;
            //character.material.color = col;
            Character.enabled = false;
            Collider.radius = 0f;
            Cloak_Count--;
        }
    }

    private void Run_Performed(InputAction.CallbackContext context)
    {
        Is_Running = !Is_Running;

        if (Is_Running && Invisble==false)
        { 
            Is_Crouching = false;
            Collider.radius = 100f; 
        }
        else if(Is_Running && Invisble)
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
        else if (Is_Crouching && Invisble == false)
        {
            Is_Running = false;
        }
        else
            Collider.radius = 75f;
    }

    private void Update()
    {
        Move_Direction = Move.ReadValue<Vector2>();
        Activation_Time += Time.deltaTime;
        if (Invisble && Activation_Time >= 3)
        {
            Invisble = false;
            //col.a = 1f;
            //character.material.color = col;
            Character.enabled = true;

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
        Vector3 MovementDirection = transform.forward * Move_Direction.y + transform.right * Move_Direction.x;
        //if (MovementDirection!= Vector3.zero) gameObject.transform.forward = Vector3.Lerp(gameObject.transform.position, MovementDirection.normalized,Rotation_Speed* Time.deltaTime);
        if (Is_Crouching)
            Rb.AddForce(Crouch_Speed * Time.deltaTime * MovementDirection,ForceMode.Force);
        else if (Is_Running)
            Rb.AddForce(Run_Speed * Time.deltaTime * MovementDirection,ForceMode.Force);
        else if (Is_Running == false && Is_Crouching == false)
            Rb.AddForce(Move_Speed * Time.deltaTime * MovementDirection,ForceMode.Force);

    }
}