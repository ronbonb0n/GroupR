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
        if (drone.fieldOfView.canSeePlayer)
        {
            return drone.alertState;
        }
        if (ReachedDestination(drone))
        {
            return drone.lookAroundState;
        }
        return drone.investigateState;
    }

    public void onEnter(DroneController drone)
    {
        Color salmon = new Color(250f / 255f, 128f / 255f, 114f / 255f);
        drone.SetLinesColor(salmon);
        drone.SetStateText("Investigate", salmon);
        drone.SetLaserColor(salmon);
        drone.navMeshAgent.destination = drone.fieldOfView.lastSeenPlayerAt;
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