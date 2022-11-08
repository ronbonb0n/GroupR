using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Notation is above each relevant code snippet
// Script requires that the player have a custom layer called "player" and a custom tag also called "player"
// This is because I wanted to minimse the number of gameobjects detected by the drones when scanning with a layermask
// And the tag allows me to check that the player is still visible and hasn't hidden behind an object - more on this below

// This is unclean code - fully expect it to be ripped apart, but it creates a baseline and a target

public class DroneBehaviours : MonoBehaviour
{
    private NavMeshAgent agent;
    public SphereCollider surveyZone;
    public SphereCollider listeningTrigger;
    public LayerMask listenLayerMask;
    public List<GameObject> hitObjects = new List<GameObject>();

    // The wait time before the drone will actually search for the player once it has transitioned to the search state
    // This gives the player time to hide - can be 0, but I think 1.5 seconds provides a good balance of leniancy and pressure to hide
    public float searchDelay;

    // The time take to pivot to the next view angle
    // If the drone fails to verify the player is present when searching (due to the player hiding)
    // The drone will trigger a circular sweep of the area
    public float scanInterval;

    // The size of the spherical ray cast, ultimately controls the field of view and distance the drone can "see"
    public float scanSize;

    //Number of times the drone will rotate (in 360 degrees) when searching
    public float numRotate;

    // Player target
    public GameObject target;

    private bool targetFound = false;
    public bool canPatrol = true;

    // Enum player states
    public enum state
    {
        Patrol,
        Search,
        Found
    }

    // Use currentState as an instance of the above states
    public state currentState;

    // Changes colour based on state so the player can see what the drone is doing
    public Light statusLight;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Set initial state to be patrol
        currentState = state.Patrol;

        // Calls the the IEnumerator function that is specific to each state - see the function at the end
        // For more details
        callState();
    }

    // If there is an overlap of the listening spherical trigger and the overlaping object has the tag "player"
    // halt movement and call the searching state and IEnumerator function
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            agent.SetDestination(this.transform.position);
            Debug.Log("Player Heard");

            // targetFound means that we don't get the drone moving back into search state 
            // when in found state - I want to manually control this within the found state
            // when the field of view of the drone is interrupted - or perhaps when the distance 
            // between player and drone exceeds a threshold
            // Boolean is set to false in the found state IEnumerator (directly below)
            if (!targetFound)
            {
                // Call search state and IEnumerator when the player's trigger sphere overlaps the drone's spherical collider
                // In this way the drone "hears" the player
                currentState = state.Search;
                callState();
            }
        }
    }

    // Different IEnumator functions that are called on state - and also check the correct state for safety
    // check call state function at bottom for more information
    // all IEnumerators require "State" at the end of their names, e.g. FoundState, PatrolState, SearchState
    IEnumerator FoundState()
    {
        while (currentState == state.Found) // boots drone out of functionality when state is no longer correct
        {
            // Found state - called when player is verified when in search state
            Debug.Log("Player Found");
            targetFound = true;
            statusLight.color = Color.red;

            // Currently doesn't do anything - just waits for as long as the public searchDelay variable allows
            yield return new WaitForSeconds(searchDelay);

            // Then returns to patrol state
            currentState = state.Patrol;
        }

        // on enum state change while loop breaks - and I call new enumerator relevant to the current state (in this case patrol)
        // with the callState() function
        callState();
        yield return null;
    }
    
    // IEnumerator called when in patrol state
    IEnumerator PatrolState()
    {
        // canPatrol will be set to false with the kill switch
        if (canPatrol)
        {
            targetFound = false;
            statusLight.color = new Color(0f, 188f/255f, 1f, 1f);
            // Debug.Log("Patrol: Enter");
            while (currentState == state.Patrol)
            {
                if (agent.remainingDistance == agent.stoppingDistance)
                {
                    // Calls the move to location function when above if statement is true
                    moveToLocation();
                }
                yield return null;
            }
            // Debug.Log("Patrol: Exit");
        }
    }

    // The janky bit of the code - called when in search state
    IEnumerator SearchState()
    {
        Debug.Log("Search: Enter");
        while (currentState == state.Search)
        {
            statusLight.color = Color.yellow;
            // Get rotation required to look at player
            Vector3 dir = target.transform.position - transform.position;
            
            // Forces y axis to be static
            dir.y = 0;

            // target rotation
            Quaternion rot = Quaternion.LookRotation(dir);
            
            // Delay for balance - increases player reaction time window
            // Drone won't act for this time, meaning the player can hide
            yield return new WaitForSeconds(searchDelay);

            // Initial scan
            // Because we're using IEnumerators we can't use the update wrapper
            // And we need some kind of frame based execution to utilise lerping between angles to get that nice smoothed rotation
            // As such I use a while loop which is regulated by the Time.delta time
            // Time delta is added to the elapsed time float - once this exceeds the public variable scanInterval while loop terminates
            // Therefore we can change the rate the drone pivot's by adjusting the scanInterval within the editor window
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
            // This returns all objects within the sphere (determined by public radius parameter)
            // That have the custom layer "Player" - should only be the player
            RaycastHit[] hits = Physics.SphereCastAll(
                origin : transform.position, radius : scanSize, direction : transform.forward,
                maxDistance : listeningTrigger.radius/2, layerMask : listenLayerMask,
                queryTriggerInteraction : QueryTriggerInteraction.UseGlobal);
            
            // Public object designated at start - do not confuse with hits array
            // Need to clear seen player for next search
            // I use another list here of hit objects to prevent drone from immediately going to found in future
            // It's ineffcient and needs cleaning up
            // Public object designated at start - do not confuse with hits array
            hitObjects.Clear();
            foreach (var hit in hits)
            {
                hitObjects.Add(hit.transform.gameObject);
                
                // For each hit within the sphere (should be just the player)
                // Ray cast from the drone in the direction of the player
                // distance of ray = radius of the listeningsphere + the scan size
                // Couldn't get line trace to work - hence the ray trace
                RaycastHit hitObject;
                if (Physics.Raycast(
                    origin: transform.position, direction : hit.transform.position - transform.position,
                    out hitObject, maxDistance : listeningTrigger.radius + scanSize, layerMask : 2147483647, // 2147483647 is apparently the integer for the "everything" layer
                    queryTriggerInteraction : QueryTriggerInteraction.Ignore)
                )
                {
                    // if the tag of hit object = the player's tag (i.e. the target) then we've been spotted
                    // otherwise not spotted
                    // This is useful as it means that if the player hides, or was just heard through a wall
                    // The drone won't "find" the player
                    if (hitObject.collider.gameObject.tag.Equals(target.tag))
                    {
                        // If spotted call state found
                        currentState = state.Found;
                        // Change state
                        callState();
                        // break out of loop and IEnumerator
                        yield break;
                    }
                }
            }
            // Rotating surviellance sequence
            // Triggers if the player is not seen
            // Dron will circle 360 degrees
            // The number of stagger rotations is public - currently 4
            // Therefore rotation amount is 90 degrees
            // Loop for rotation is numrotate -1 as the first scan has already occured (directly infront of the drone)
            float rotateAmount = 360 / numRotate;
            for (int i = 0; i < numRotate - 1; i++)
            {
                Debug.Log($"Scan: {i}");

                // New rotation is is current rotation * the rotateAmount * to the upward vector
                Quaternion newRotation = transform.rotation;
                newRotation *= Quaternion.Euler(Vector3.up * rotateAmount);

                // Another janky while loop with delta time
                // lerps between current rotation and desired rotation
                elapsedTime = 0;
                while (elapsedTime < scanInterval)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, elapsedTime/scanInterval);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }  
                // Confirm that final rotation is correct
                transform.rotation = newRotation;
                
                // Exact same scan procedure as above - copied and pasted
                // Couldn't figure out how to implement an object based function within the IEnumerator
                // And ran out of time - full expect this code to be ripped apart anyway
                hits = Physics.SphereCastAll(
                    origin : transform.position, radius : scanSize, direction : transform.forward,
                    maxDistance : listeningTrigger.radius/2, layerMask : listenLayerMask,
                    queryTriggerInteraction : QueryTriggerInteraction.UseGlobal);
                
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

                // Again just gives player reaction time
                yield return new WaitForSeconds(searchDelay);
            }

            // If player is not found at end of seach state - call patrol state
            currentState = state.Patrol;
            yield return null;
        }
        Debug.Log("Search: Exit");
        // Call state function to change back to patrol IEnumerator
        callState();
    }

    void callState()
    {
        // gets the current state as an enum and converts to string, then concates "State" on the end of this string
        string methodName = currentState.ToString() + "State";
        // Debug.Log(methodName);
        System.Reflection.MethodInfo info =
            GetType().GetMethod(methodName,
                                System.Reflection.BindingFlags.NonPublic |
                                System.Reflection.BindingFlags.Instance);
        // Calls the IEnumarator with the above methods name as a coroutine
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
