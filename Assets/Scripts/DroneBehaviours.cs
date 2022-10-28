using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.UI.ScrollRect;

public class DroneBehaviours : MonoBehaviour
{
    private NavMeshAgent agent;
    public SphereCollider surveyZone;

    // Can see you
    // public MeshCollider visualVolume;
    // Can hear you
    public SphereCollider listeningTrigger;

    public int searchDelay;

    // Searching booleans
    private bool isSearching = false;
    // private bool isPatrolling = false;
    public bool canPatrol = true;

    private enum state
    {
        Patrol,
        Search,
        Found
    }

    private state currentState;

    //Number of times the drone will rotate (in 360 degrees) when searching
    public float numRotate;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        // visualVolume = GetComponent<MeshCollider>();
        currentState = state.Patrol;
        callState();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            canPatrol = false;
            agent.SetDestination(this.transform.position);
            Debug.Log("Player Heard");
            currentState = state.Search;
            callState();
        }
    }

    // Different IEnumator events that are called on state - and also check the correct state for safety
    IEnumerator PatrolState()
    {
        Debug.Log("Patrol: Enter");
        while (currentState == state.Patrol)
        {
            if (agent.remainingDistance == agent.stoppingDistance)
            {
                moveToLocation();
            }
            yield return 0;
        }
        Debug.Log("Patrol: Exit");
    }

    IEnumerator SearchState()
    {
        Debug.Log("Search: Enter");
        while (currentState == state.Search)
        {
            Debug.Log("Waiting");
            yield return new WaitForSeconds(searchDelay);
            Debug.Log("Waited");

            Quaternion newRotation = transform.rotation;
            float rotateAmount = 360 / numRotate;
            Debug.Log("Searching Start");

            for (int i = 0; i < numRotate; i++)
            {
                newRotation *= Quaternion.Euler(Vector3.up * rotateAmount);
                transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * searchDelay);
                
                Debug.Log($"Search stage: {i}");
                yield return new WaitForSeconds(searchDelay);
            }
            currentState = state.Patrol;
            yield return 0;
        }
        Debug.Log("Search: Exit");
        callState();
    }

    void callState()
    {
        string methodName = currentState.ToString() + "State";
        Debug.Log(methodName);
        System.Reflection.MethodInfo info =
            GetType().GetMethod(methodName,
                                System.Reflection.BindingFlags.NonPublic |
                                System.Reflection.BindingFlags.Instance);
        StartCoroutine((IEnumerator)info.Invoke(this, null));
    }

    void moveToLocation()
    {
        Vector3 centrePoint = surveyZone.bounds.center;
        float range = surveyZone.radius;

        Vector3 point;
        if (RandomPoint(centrePoint, range, out point))
        {
            Debug.DrawRay(point, Vector3.up, Color.blue, 2.0f);
            agent.SetDestination(point);
        }
    }
    // Used by moveToLocation()
    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range;
        // Vector3 playerLocation = this.transform.position;
        // randomPoint.Set(randomPoint.x, playerLocation.y, randomPoint.z);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 2.0f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        // Debug.Log("Not Available");
        return false;
    }
}
