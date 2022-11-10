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
            return drone.alertState;
        }
        if (drone.lookAroundCountDownTimer <= 0)
        {
            return drone.patrolState;
        }
        drone.lookAroundCountDownTimer -= Time.deltaTime;
        return drone.lookAroundState;
    }

    public void onEnter(DroneController drone)
    {
        drone.SetLinesColor(Color.green);
        drone.SetStateText("Look Around", Color.green);
        drone.SetLaserColor(Color.green);
        drone.lookAroundCountDownTimer = Random.Range(1.0f, 2.0f);
    }

    public void onExit(DroneController drone)
    {
        drone.lookAroundCountDownTimer = 0;
    }
}