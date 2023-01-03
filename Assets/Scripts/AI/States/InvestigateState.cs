using UnityEngine;
using UnityEngine.AI;

public class InvestigateState : IDroneState
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
        if (drone.senses.canSeePlayer)
        {
            return drone.alertState;
        }
        if (ReachedDestination(drone))
        {
            return drone.lookAroundState;
        }
        drone.navMeshAgent.destination = drone.senses.lastSpottedPlayerAt;
        return drone.investigateState;
    }

    public void onEnter(DroneController drone)
    {
        Color salmon = new Color(250f / 255f, 128f / 255f, 114f / 255f);
        drone.SetStateText("Investigate", salmon);
        drone.SetScannerColor(salmon);
        drone.navMeshAgent.destination = drone.senses.lastSpottedPlayerAt;
    }

    public void onExit(DroneController drone)
    {
        drone.navMeshAgent.ResetPath();
    }

    private bool ReachedDestination(DroneController drone)
    {
        return drone.navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete && drone.navMeshAgent.remainingDistance == 0;
    }
}