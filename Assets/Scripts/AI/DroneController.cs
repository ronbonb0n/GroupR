using TMPro;
using UnityEngine;
using UnityEngine.AI;

// reference: The State Pattern (C# and Unity) - Finite State Machine https://www.youtube.com/watch?v=nnrOhb5UdRc
public class DroneController : MonoBehaviour
{
    public Senses senses;
    public NavMeshAgent navMeshAgent;
    public GameObject body;
    public GameObject skinnedMesh;
    [SerializeField] private string currentStateName;

    private IDroneState currentState;
    private IdleState idleState = new();
    public ChaseState chaseState = new();
    public LookAroundState lookAroundState = new();
    public PatrolState patrolState = new();
    public AlertState alertState = new();
    public InvestigateState investigateState = new();
    public DeactivatedState deactivatedState = new();
    public StunnedState stunnedState = new();

    public bool isActivated = true;
    public bool isStunned = false;
    public Vector3 initialPosition;
    public float patrolRadius = 20;
    public float lookAroundCountDownTimer = 0;
    public float alertCountDown = 1f;
    public float alertCountDownTimer = 0;
    public float stunnedCountDown = 5f;
    public float stunnedCountDownTimer = 0;
    
    public GameObject player;
    private Material scannerMaterial;
    private Material droneMaterial;
    public LevelCanvasControls canvasControl;
    //public Animator animator;

    // Audio
    public AudioSource droneRotorAudio;
    public AudioSource droneAlertAudio;
    public AudioSource droneInvestigateAudio;

    private void Awake()
    {
        initialPosition = transform.position;
        navMeshAgent = GetComponent<NavMeshAgent>();
        senses = GetComponentInChildren<Senses>();
        player = GameObject.FindGameObjectWithTag("Player");

        scannerMaterial = transform.Find(
            "Body/Drone_Scanning_Cone").gameObject.GetComponent<
                Renderer>().material;

        droneMaterial = transform.Find(
            "Forward_Looking_Scout_Drone/Skinned_Mesh").gameObject.GetComponent<
                SkinnedMeshRenderer>().material;

        // Debug.Log(scannerMaterial.ToString());
        
        canvasControl = GameObject.Find("CanvasControls").GetComponent<LevelCanvasControls>();
    }

    private void OnEnable()
    {
        currentState = idleState;
    }
    
    // Start is called before the first frame update
    private void Start()
    {
        body = transform.GetChild(0).gameObject;
        skinnedMesh = transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    private void Update()
    {
        var nextState = currentState.DoState(this);
        if (currentState == nextState)
        {
            currentState = nextState;
        }
        else
        {
            currentState.onExit(this);
            currentState = nextState;
            currentState.onEnter(this);
        }

        currentStateName = currentState.ToString();
    }
    
    public void SetStateText(string text, Color color)
    {
        var stateText = transform.GetComponentInChildren<TextMeshPro>();
        stateText.text = text;
        stateText.color = color;
    }
    
    public void SetScannerColor(Color color)
    {
        color.a = 1.0f;
        droneMaterial.SetColor("_EmissionColour", color);
        scannerMaterial.SetColor("_PulseColour", color);
    }

    public void LevelOver()
    {
        canvasControl.onLevelLost();
    }
}
