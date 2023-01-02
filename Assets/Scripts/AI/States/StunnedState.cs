using UnityEngine;

public class StunnedState : IDroneState
{
    public IDroneState DoState(DroneController drone)
    {
        if (!drone.isActivated)
        {
            return drone.deactivatedState;
        }
        if (drone.stunnedCountDownTimer <= 0)
        {
            return drone.lookAroundState;
        }
        drone.stunnedCountDownTimer -= Time.deltaTime;
        return drone.stunnedState;
    }

    public void onEnter(DroneController drone)
    {
        drone.SetStateText("Stunned", Color.blue);
        drone.SetScannerColor(Color.blue);
        drone.stunnedCountDownTimer = drone.stunnedCountDown;
    }

    public void onExit(DroneController drone)
    {
        drone.stunnedCountDownTimer = 0;
        drone.isStunned = false;
    }
}