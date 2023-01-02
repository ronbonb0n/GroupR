using UnityEngine;

// reference: Unity Basics - Move towards and follow target https://www.youtube.com/watch?v=wp8m6xyIPtE
public class ChaseState : IDroneState
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
        if (drone.senses.canSeePlayer)
        {
            // face the player
            drone.transform.forward = new Vector3(drone.player.transform.position.x - drone.transform.position.x, 0,drone.player.transform.position.z - drone.transform.position.z);
            // stay still and end the level if caught the player
            if (HasCaughtPlayer(drone))
            {
                drone.LevelOver();
            }
            // chase the player
            else
            {
                drone.transform.position = Vector3.MoveTowards(drone.transform.position, drone.player.transform.position, 9 * Time.deltaTime);
            }
            return drone.chaseState;
        }
        return drone.investigateState;
    }

    public void onEnter(DroneController drone)
    {
        drone.SetLinesColor(Color.yellow);
        drone.SetStateText("Chase", Color.yellow);
        drone.SetLaserColor(Color.yellow);
        drone.navMeshAgent.ResetPath();
        drone.senses.sightAngle = 360;
    }

    public void onExit(DroneController drone)
    {
        drone.navMeshAgent.ResetPath();
    }

    private Vector3 GetPlayerPosition(DroneController drone)
    {
        return drone.player.transform.position;
    }

    private bool HasCaughtPlayer(DroneController drone)
    {
        return Vector2.Distance(new Vector2(drone.transform.position.x, drone.transform.position.z), new Vector2(drone.player.transform.position.x, drone.player.transform.position.z)
        ) <= 3;
    }
}