using UnityEngine;

public class LookAroundState : IDroneState
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
        if (drone.lookAroundCountDown <= 0)
        {
            return drone.patrolState;
        }
        drone.lookAroundCountDown -= Time.deltaTime;
        return drone.lookAroundState;
    }

    public void onEnter(DroneController drone)
    {
        drone.lookAroundCountDown = 2;
    }

    public void onExit(DroneController drone)
    {
        drone.lookAroundCountDown = 0;
    }
}