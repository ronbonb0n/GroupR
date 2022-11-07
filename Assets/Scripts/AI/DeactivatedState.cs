using Unity.VisualScripting;
using UnityEngine;

public class DeactivatedState : IDroneState
{
    public IDroneState DoState(DroneController drone)
    {
        return drone.deactivatedState;
    }

    public void onEnter(DroneController drone)
    {
        drone.SetLinesColor(Color.red);
        drone.navMeshAgent.enabled = false;
        drone.body.AddComponent<Rigidbody>();
    }

    public void onExit(DroneController drone)
    {
    }
}