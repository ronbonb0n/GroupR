using TMPro;
using UnityEngine;
using UnityEngine.AI;

// reference: The State Pattern (C# and Unity) - Finite State Machine https://www.youtube.com/watch?v=nnrOhb5UdRc
public class DroneController : MonoBehaviour
{
    public Senses senses;
    public NavMeshAgent navMeshAgent;
    public GameObject body;
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
    private Material laserMaterial;
    public LevelCanvasControls canvasControl;

    private void Awake()
    {
        initialPosition = transform.position;
        navMeshAgent = GetComponent<NavMeshAgent>();
        senses = GetComponentInChildren<Senses>();
        player = GameObject.FindGameObjectWithTag("Player");
        laserMaterial = transform.Find("Body/Laser").gameObject.GetComponent<Renderer>().material;
    }

    private void OnEnable()
    {
        currentState = idleState;
    }
    
    // Start is called before the first frame update
    private void Start()
    {
        body = transform.GetChild(0).gameObject;
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

    public void SetLinesColor(Color color)
    {
        var lines = transform.GetComponentsInChildren<LineRenderer>();
        foreach (var line in lines)
        {
            line.SetColors(color, color);
        }
    }
    
    public void SetStateText(string text, Color color)
    {
        var stateText = transform.GetComponentInChildren<TextMeshPro>();
        stateText.text = text;
        stateText.color = color;
    }
    
    public void SetLaserColor(Color color)
    {
        color.a = 0.4f;
        laserMaterial.color = color;
    }

    public void LevelOver()
    {
        canvasControl.onLevelLost();
    }
}
