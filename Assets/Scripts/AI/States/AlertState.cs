using UnityEngine;

public class AlertState : IDroneState
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
        if (drone.alertCountDownTimer <= 0)
        {
            if (drone.senses.canSeePlayer)
            {
                return drone.chaseState;
            }
            return drone.investigateState;
        }
        drone.alertCountDownTimer -= Time.deltaTime;
        return drone.alertState;
    }

    public void onEnter(DroneController drone)
    {
        Color orange = new Color(1, 172f / 255f, 28f / 255f);
        drone.SetStateText("Alerted", orange);
        drone.SetScannerColor(orange);
        drone.alertCountDownTimer = drone.alertCountDown;
    }

    public void onExit(DroneController drone)
    {
        drone.alertCountDownTimer = 0;
    }
}