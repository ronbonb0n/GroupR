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
        Color darkOrange = new Color(230f / 255f, 126f / 255f, 0);
        drone.SetLinesColor(darkOrange);
        drone.SetStateText("Stunned", darkOrange);
        drone.SetLaserColor(darkOrange);
        drone.stunnedCountDownTimer = drone.stunnedCountDown;
    }

    public void onExit(DroneController drone)
    {
        drone.stunnedCountDownTimer = 0;
        drone.isStunned = false;
    }
}