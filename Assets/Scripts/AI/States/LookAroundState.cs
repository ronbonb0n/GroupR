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
        if (drone.isStunned)
        {
            return drone.stunnedState;
        }
        if (drone.senses.canSeePlayer || drone.senses.canHearPlayer || drone.senses.isAttracted)
        {
            if (drone.senses.isAttracted)
            {
                drone.senses.isAttracted = false;
            }
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
        drone.SetStateText("Look Around", Color.green);
        drone.SetScannerColor(Color.green);
        drone.lookAroundCountDownTimer = Random.Range(2.5f, 3.5f);
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
            // Debug.DrawRay(drone.senses.transform.position, direction * 15, Color.green, 5f);
            Physics.Raycast(drone.senses.transform.position, direction * 15, out hit);
            if (hit.distance > maxDistance)
            {
                maxDistance = hit.distance;
                lookAt = direction;
            }
        }
        // Debug.DrawRay(drone.senses.transform.position, lookAt * 15, Color.magenta, 5f);
    }
}