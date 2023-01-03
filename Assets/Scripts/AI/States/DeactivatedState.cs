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
        drone.SetStateText("Deactivated", Color.red);
        drone.SetScannerColor(Color.red);
        drone.navMeshAgent.enabled = false;
        drone.skinnedMesh.AddComponent<Rigidbody>();

        // Deactivate scanners and text
        for(int i=0; i<drone.body.transform.childCount; i++)
        {
            drone.body.transform.GetChild(i).gameObject.SetActive(false);
        }
        drone.droneRotorAudio.enabled= false;
    }

    public void onExit(DroneController drone)
    {
    }
}