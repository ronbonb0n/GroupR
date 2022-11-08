using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class RandomMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    public SphereCollider surveyZone;

    // Can see you
    // public MeshCollider visualVolume;
    // Can hear you
    public SphereCollider listeningTrigger;

    private float time;
    public float searchDelay;

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
        time = 0f;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player")
        {
            canPatrol = false;
            agent.SetDestination(this.transform.position);
            Debug.Log("Player Heard");
            currentState = state.Search;
        }
    }
    
    void Update()
    {
        // patrol state
        if (currentState == state.Patrol)
        {
            if(agent.remainingDistance == agent.stoppingDistance)
            {
                moveToLocation();
            }   
        }

        if (currentState == state.Search && !isSearching)
        {   
            isSearching = true;
            // Pause - give the player some time to get out - similar concept to invisible overhang in platformers
            time = time + 1f * Time.deltaTime;
            if (time >= searchDelay)
            {
                Debug.Log("Searched");
                currentState = state.Patrol;
                isSearching = false;
                time = 0f;
            }
            
        }

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



    void search()
    {
        Quaternion newRotation = transform.rotation;
        float rotateAmount = 360 / numRotate;
        for (int j = 0; j < numRotate; j++)
        {
            if (j != 0)
            {
                time = time + 1f * Time.deltaTime;
                if (time >= searchDelay)
                {
                    newRotation *= Quaternion.Euler(Vector3.up * rotateAmount);
                    Debug.Log(newRotation);
                    transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * searchDelay);
                }
            }
            else
            {
                newRotation *= Quaternion.Euler(Vector3.up * rotateAmount);
                Debug.Log(newRotation);
                transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * searchDelay);
            }
        }
    }
}
