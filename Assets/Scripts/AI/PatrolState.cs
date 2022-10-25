using UnityEngine.AI;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class PatrolState : IDroneState
{
    public IDroneState DoState(DroneController drone)
    {
        if (!drone.isActivated)
        {
            return drone.deactivatedState;
        }
        if (drone.fieldOfView.canSeePlayer)
        {
            return drone.chaseState;
        }
        if (ReachedDestination(drone))
        {
            return drone.lookAroundState;
        }
        return drone.patrolState;
    }

    public void onEnter(DroneController drone)
    {
        drone.navMeshAgent.destination = GetRandomLocation(drone);
    }

    public void onExit(DroneController drone)
    {
        drone.navMeshAgent.ResetPath();
    }

    private Vector3 GetRandomLocation(DroneController drone)
    {
        Vector3 randomLocation = Random.insideUnitSphere * drone.patrolRadius;
        randomLocation += drone.initialPosition;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomLocation, out hit, drone.patrolRadius, 1);
        return hit.position;
    }

    private bool ReachedDestination(DroneController drone)
    {
        return drone.navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete && drone.navMeshAgent.remainingDistance == 0;
    }
}