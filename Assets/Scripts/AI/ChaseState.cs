using UnityEngine;

public class ChaseState : IDroneState
{
    public IDroneState DoState(DroneController drone)
    {
        if (!drone.isActivated)
        {
            return drone.deactivatedState;
        }
        if (drone.fieldOfView.canSeePlayer)
        {
            drone.transform.position = Vector3.MoveTowards(drone.transform.position, drone.player.transform.position, 15 * Time.deltaTime);
            drone.transform.forward = drone.player.transform.position - drone.transform.position;
            return drone.chaseState;
        }
        else
        {
            // return drone.patrolState;
            return drone.chaseState;
        }
    }

    public void onEnter(DroneController drone)
    {
        drone.navMeshAgent.ResetPath();
    }

    public void onExit(DroneController drone)
    {
        drone.navMeshAgent.ResetPath();
    }

    private Vector3 GetPlayerPosition(DroneController drone)
    {
        return drone.player.transform.position;
    }
}