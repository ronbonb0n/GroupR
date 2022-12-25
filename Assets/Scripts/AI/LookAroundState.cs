using UnityEngine;

public class LookAroundState : IDroneState
{
    private Vector3 lookAt;
    
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
        drone.transform.forward = Vector3.RotateTowards(drone.transform.forward, lookAt, 2 * Time.deltaTime, 0.8f * Time.deltaTime);
        drone.lookAroundCountDownTimer -= Time.deltaTime;
        return drone.lookAroundState;
    }

    public void onEnter(DroneController drone)
    {
        drone.SetLinesColor(Color.green);
        drone.SetStateText("Look Around", Color.green);
        drone.SetLaserColor(Color.green);
        drone.lookAroundCountDownTimer = Random.Range(1.0f, 2.0f);
        GetLookDirection(drone);
    }

    public void onExit(DroneController drone)
    {
        drone.lookAroundCountDownTimer = 0;
        lookAt = Vector3.forward;
    }

    private void GetLookDirection(DroneController drone)
    {
        float angle = 0;
        float maxDistance = -1;
        RaycastHit hit;
        for (int i = 0; i < 4; i++, angle += 90)
        {
            Vector3 direction = Quaternion.AngleAxis(angle, Vector3.up) * drone.transform.forward;
            Debug.DrawRay(drone.fieldOfView.transform.position, direction * 15, Color.green, 5f);
            Physics.Raycast(drone.fieldOfView.transform.position, direction * 15, out hit);
            if (hit.distance > maxDistance)
            {
                maxDistance = hit.distance;
                lookAt = direction;
            }
        }
        Debug.DrawRay(drone.fieldOfView.transform.position, lookAt * 15, Color.magenta, 5f);
    }
}