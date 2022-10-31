using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DroneBehaviours : MonoBehaviour
{
    private NavMeshAgent agent;
    public SphereCollider surveyZone;
    public SphereCollider listeningTrigger;
    public LayerMask listenLayerMask;
    public List<GameObject> hitObjects = new List<GameObject>();

    public float searchDelay;
    public float scanInterval;
    public float scanSize;
    //Number of times the drone will rotate (in 360 degrees) when searching
    public float numRotate;
    public GameObject target;

    private bool isSearching = false;
    public bool canPatrol = true;

    private enum state
    {
        Patrol,
        Search,
        Found
    }
    private state currentState;

    public Light statusLight;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        // Light statusLight = GetComponent<Light>();
        currentState = state.Patrol;
        callState();
        // Debug.Log(listenLayerMask.value);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            agent.SetDestination(this.transform.position);
            Debug.Log("Player Heard");

            currentState = state.Search;
            callState();
        }
    }

    // Different IEnumator events that are called on state - and also check the correct state for safety
    IEnumerator FoundState()
    {
        Debug.Log("Player Found");
        statusLight.color = Color.red;

        yield return new WaitForSeconds(searchDelay * 2);

        currentState = state.Patrol;
        yield return null;
    }
    
    IEnumerator PatrolState()
    {
        if (canPatrol)
        {
            statusLight.color = Color.cyan;
            // Debug.Log("Patrol: Enter");
            while (currentState == state.Patrol)
            {
                if (agent.remainingDistance == agent.stoppingDistance)
                {
                    moveToLocation();
                }
                yield return null;
            }
            // Debug.Log("Patrol: Exit");
        }
    }

    IEnumerator SearchState()
    {
        Debug.Log("Search: Enter");
        while (currentState == state.Search)
        {
            statusLight.color = Color.yellow;
            // Get rotation to look at player
            Vector3 dir = target.transform.position - transform.position;
            dir.y = 0;
            Quaternion rot = Quaternion.LookRotation(dir);
            
            // Delay for balance - increases player reaction time window
            yield return new WaitForSeconds(searchDelay);

            // Initial scan
            float elapsedTime = 0;
            while (elapsedTime < scanInterval)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, rot, elapsedTime/scanInterval);
                elapsedTime += Time.deltaTime;
            
                yield return null;
            }  
            // Confirm that final rotation is correct
            transform.rotation = rot;

            // Fire off first ray sphere to check if player is infront
            RaycastHit[] hits = Physics.SphereCastAll(
                origin : transform.position, radius : scanSize, direction : transform.forward,
                maxDistance : listeningTrigger.radius/2, layerMask : listenLayerMask,
                queryTriggerInteraction : QueryTriggerInteraction.UseGlobal);
            
            // Public object designated at start - do not confuse with hits array
            hitObjects.Clear();
            foreach (var hit in hits)
            {
                hitObjects.Add(hit.transform.gameObject);
                RaycastHit hitObject;
                if (Physics.Raycast(
                    origin: transform.position, direction : hit.transform.position - transform.position,
                    out hitObject, maxDistance : 250f, layerMask : 2147483647,
                    queryTriggerInteraction : QueryTriggerInteraction.Ignore)
                )
                {
                    if (hitObject.collider.gameObject.tag.Equals(target.tag))
                    {
                        currentState = state.Found;
                        callState();
                        yield break;
                    }
                }
            }
            // Rotating surviellance sequence
            float rotateAmount = 360 / numRotate;
            for (int i = 0; i < numRotate - 1; i++)
            {
                Debug.Log($"Scan: {i}");
                Quaternion newRotation = transform.rotation;
                newRotation *= Quaternion.Euler(Vector3.up * rotateAmount);

                elapsedTime = 0;
                while (elapsedTime < scanInterval)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, elapsedTime/scanInterval);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }  
                // Confirm that final rotation is correct
                transform.rotation = newRotation;
                
                hits = Physics.SphereCastAll(
                    origin : transform.position, radius : scanSize, direction : transform.forward,
                    maxDistance : listeningTrigger.radius/2, layerMask : listenLayerMask,
                    queryTriggerInteraction : QueryTriggerInteraction.UseGlobal);
                
                // Public object designated at start - do not confuse with hits array
                hitObjects.Clear();
                foreach (var hit in hits)
                {
                    hitObjects.Add(hit.transform.gameObject);
                    RaycastHit hitObject;
                    if (Physics.Raycast(
                        origin: transform.position, direction : hit.transform.position - transform.position,
                        out hitObject, maxDistance : 250f, layerMask : 2147483647,
                        queryTriggerInteraction : QueryTriggerInteraction.Ignore)
                    )
                    {
                        if (hitObject.collider.gameObject.tag.Equals(target.tag))
                        {
                            currentState = state.Found;
                            callState();
                            yield break;
                        }
                    }
                }

                yield return new WaitForSeconds(0.1f);
            }
            
            currentState = state.Patrol;
            yield return null;
        }
        Debug.Log("Search: Exit");
        callState();
    }

    void callState()
    {
        string methodName = currentState.ToString() + "State";
        // Debug.Log(methodName);
        System.Reflection.MethodInfo info =
            GetType().GetMethod(methodName,
                                System.Reflection.BindingFlags.NonPublic |
                                System.Reflection.BindingFlags.Instance);
        StartCoroutine((IEnumerator)info.Invoke(this, null));
    }

    // Used in patrol coroutine
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
