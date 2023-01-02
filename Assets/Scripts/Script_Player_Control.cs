using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class Script_Player_Control : MonoBehaviour
{
    public Rigidbody Rb;
    public float Crouch_Speed = 1000f;
    public float Move_Speed = 1400f;
    public float Run_Speed = 2500f;
    public float Rotation_Speed = 70f;
    public Player_Controls Player_Input;
    private InputAction Move;
    private InputAction Run;
    private InputAction Crouch;
    private Vector2 Move_Direction;
    public bool Is_Running = false;
    public bool Is_Crouching = false;
    private InputAction Cloak;
    public SkinnedMeshRenderer Character;
    public Material CharacterMaterial;
    //private SpriteRenderer character; USE IT AFTER SPRITES ARE ADDED
    //private Color col; USE IT FOR TRANSLUCENT COLOR
    private float Activation_Time;
    public bool Invisble;
    public Transform frontObj;
    private InputAction EMP;
    private InputAction Decoy;
    public float Throw_Force = 40f;
    public GameObject EMP_Prefab;
    public GameObject Decoy_Prefab;
    public GameObject levelCanvasControlsObj;
    public LevelCanvasControls levelCanvasControls;
    private Player_Animation animator;
    private GameObject mainCam;

    void Start()
    {
        Rb = GetComponent<Rigidbody>();
        //character = GetComponent<SpriteRenderer>();
        // Character = GetComponent<MeshRenderer>();
        Activation_Time = 0;
        Invisble = false;
        levelCanvasControlsObj = GameObject.Find("CanvasControls");
        levelCanvasControls = levelCanvasControlsObj.GetComponent<LevelCanvasControls>();
        //col = character.material.color;
        animator = GetComponent<Player_Animation>();
        mainCam = GameObject.Find("Main Camera");
        CharacterMaterial = Character.sharedMaterial;
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
            animator.Throw();
            GameObject EMP = Instantiate(EMP_Prefab, transform.position, transform.rotation);
            Rigidbody RB = EMP.GetComponent<Rigidbody>();
            RB.AddForce(transform.forward * Throw_Force, ForceMode.VelocityChange);
            InventoryManager.instance.itemDecrement("EMP");
            levelCanvasControls.onItemUse();
        }
    }

    private void Decoy_Performed(InputAction.CallbackContext context)
    {
        if (InventoryManager.instance.getItemCount("DECOY") >= 1)
        {
            GameObject Decoy = Instantiate(Decoy_Prefab, transform.position, transform.rotation);
            Rigidbody RB = Decoy.GetComponent<Rigidbody>();
            RB.AddForce(transform.forward * Throw_Force, ForceMode.VelocityChange);
            InventoryManager.instance.itemDecrement("DECOY");
            levelCanvasControls.onItemUse();
        }
    }

    private void Cloak_Performed(InputAction.CallbackContext context)
    {
        if (Invisble == false && InventoryManager.instance.getItemCount("CLOAK") >= 1)
        {
            Invisble = true;
            Activation_Time = 0;
            //col.a = 0.2f;
            //character.material.color = col;
            CharacterMaterial.SetInt("_isCloaking", 1);
            InventoryManager.instance.itemDecrement("CLOAK");
            levelCanvasControls.onItemUse();
        }
    }

    private void Run_Performed(InputAction.CallbackContext context)
    {
        Is_Running = !Is_Running;

        if (Is_Running && Invisble==false)
        { 
            Is_Crouching = false;
        }
        else if(Is_Running && Invisble)
        {
            Is_Crouching = false;
        }
        
        //ANIMATE CALL 
        animator.Running(Is_Running);
        animator.Crouching(Is_Crouching);
    }

    private void Crouch_Performed(InputAction.CallbackContext context)
    {
        Is_Crouching = !Is_Crouching;

        if (Is_Crouching)
        {
            Is_Running = false;
        }
        else if (Is_Crouching && Invisble == false)
        {
            Is_Running = false;
        }
        
        //ANIMATE CALL
        animator.Crouching(Is_Crouching);
        animator.Running(Is_Running);
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
            //Character.enabled = true;
            CharacterMaterial.SetInt("_isCloaking", 0);
            
        }
    }

    private void FixedUpdate()
    {
        Vector3 viewDir = Vector3.Cross(mainCam.transform.right, Vector3.up);
        transform.forward = viewDir.normalized;
        Vector3 MovementDirection = transform.forward * Move_Direction.y + transform.right * Move_Direction.x;
        if (MovementDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(MovementDirection);
        }

        //if (MovementDirection!= Vector3.zero) gameObject.transform.forward = Vector3.Lerp(gameObject.transform.position, MovementDirection.normalized,Rotation_Speed* Time.deltaTime);
        if (Is_Crouching)
            Rb.AddForce(Crouch_Speed * Time.deltaTime * MovementDirection,ForceMode.Force);
        else if (Is_Running)
            Rb.AddForce(Run_Speed * Time.deltaTime * MovementDirection,ForceMode.Force);
        else if (Is_Running == false && Is_Crouching == false)
            Rb.AddForce(Move_Speed * Time.deltaTime * MovementDirection,ForceMode.Force);

        //ANIMATE CALL
        if (Move_Direction.x == 0 && Move_Direction.y == 0)
        {
            animator.Walking(false);
        }
        else { animator.Walking(true); }

    }
}