using UnityEngine;

public class AlertState : IDroneState
{
    public IDroneState DoState(DroneController drone)
    {
        if (drone.alertCountDownTimer <= 0)
        {
            if (drone.fieldOfView.canSeePlayer)
            {
                return drone.chaseState;
            }
            return drone.patrolState;
        }
        drone.alertCountDownTimer -= Time.deltaTime;
        return drone.alertState;
    }

    public void onEnter(DroneController drone)
    {
        Color orange = new Color(1, 172f / 255f, 28f / 255f);
        drone.SetLinesColor(orange);
        drone.SetStateText("Alerted", orange);
        drone.SetLaserColor(orange);
        drone.alertCountDownTimer = drone.alertCountDown;
    }

    public void onExit(DroneController drone)
    {
        drone.alertCountDownTimer = 0;
    }
}