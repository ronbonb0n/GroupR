using UnityEngine;
using UnityEngine.AI;

public class DroneController : MonoBehaviour
{
    public FieldOfView fieldOfView;
    [SerializeField] public Transform movePositionTransform;
    public NavMeshAgent navMeshAgent;
    [SerializeField] private string currentStateName;

    private IDroneState currentState;
    private IdleState idleState = new();
    public ChaseState chaseState = new();
    public LookAroundState lookAroundState = new();
    public PatrolState patrolState = new();
    public DeactivatedState deactivatedState = new();

    public bool isActivated = true;
    public Vector3 initialPosition;
    public float patrolRadius = 15;
    public float lookAroundCountDown = 0;
    
    public GameObject player;

    private void Awake()
    {
        initialPosition = transform.position;
        navMeshAgent = GetComponent<NavMeshAgent>();
        fieldOfView = GetComponent<FieldOfView>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnEnable()
    {
        currentState = idleState;
    }
    
    // Start is called before the first frame update
    private void Start()
    {
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
}