using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; //important

public class RandomMovement : MonoBehaviour //don't forget to change the script name if you haven't
{
    private NavMeshAgent agent;
    public SphereCollider surveyZone;

    // Can see you
    // public MeshCollider visualVolume;
    // Can here you
    public SphereCollider listeningTrigger;

    public float searchDelay;
    // Searching booleans
    private bool isSearching = false;
    private bool isPatrolling = false;
    public bool canPatrol = true;
    
    private enum state
    {
        Patrol,
        Search,
        Found
    }

    private state currentState;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        // visualVolume = GetComponent<MeshCollider>();
        currentState = state.Patrol;        
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
            currentState = state.Patrol;
            isSearching = false;
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
