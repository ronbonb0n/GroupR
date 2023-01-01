using UnityEngine;
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
        if (drone.isStunned)
        {
            return drone.stunnedState;
        }
        if (drone.senses.canSeePlayer || drone.senses.canHearPlayer || drone.senses.isAttracted)
        {
            if (drone.senses.isAttracted)
            {
                drone.senses.isAttracted = false;
            }
            return drone.alertState;
        }
        if (ReachedDestination(drone))
        {
            return drone.lookAroundState;
        }
        return drone.patrolState;
    }

    public void onEnter(DroneController drone)
    {
        drone.SetLinesColor(Color.green);
        drone.SetStateText("Patrol", Color.green);
        drone.SetLaserColor(Color.green);
        drone.navMeshAgent.destination = GetRandomLocation(drone);
    }

    public void onExit(DroneController drone)
    {
        drone.navMeshAgent.ResetPath();
    }

    private Vector3 GetRandomLocation(DroneController drone)
    {
        Vector3 oPos = drone.initialPosition;
        float radius = drone.patrolRadius;
        Vector3 randomLocation = new Vector3(Random.Range(oPos.x - radius, oPos.x + radius), 0, Random.Range(oPos.z - radius, oPos.z + radius));
        NavMeshHit hit;
        NavMesh.SamplePosition(randomLocation, out hit, drone.patrolRadius, 1);
        return hit.position;
    }

    private bool ReachedDestination(DroneController drone)
    {
        return drone.navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete && drone.navMeshAgent.remainingDistance == 0;
    }
}