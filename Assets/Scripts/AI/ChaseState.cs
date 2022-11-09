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
            if (IsOverPlayer(drone))
            {
                drone.LevelOver();
            }
            drone.transform.position = Vector3.MoveTowards(drone.transform.position, drone.player.transform.position, 9 * Time.deltaTime);
            drone.transform.forward = new Vector3(drone.player.transform.position.x - drone.transform.position.x, 0, drone.player.transform.position.z - drone.transform.position.z);
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
        drone.SetLinesColor(Color.yellow);
        drone.SetStateText("Chase", Color.yellow);
        drone.SetLaserColor(Color.yellow);
        drone.navMeshAgent.ResetPath();
        drone.fieldOfView.angle = 360;
    }

    public void onExit(DroneController drone)
    {
        drone.navMeshAgent.ResetPath();
    }

    private Vector3 GetPlayerPosition(DroneController drone)
    {
        return drone.player.transform.position;
    }

    private bool IsOverPlayer(DroneController drone)
    {
        return Vector3.Distance(drone.transform.position, drone.player.transform.position) <= 5;
    }
}