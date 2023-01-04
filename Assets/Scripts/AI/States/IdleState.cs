public class IdleState : IDroneState
{
    public IDroneState DoState(DroneController drone)
    {
        return drone.patrolState;
    }

    public void onEnter(DroneController drone)
    {
    }

    public void onExit(DroneController drone)
    {
    }
}