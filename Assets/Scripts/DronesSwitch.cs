using UnityEngine;

public class DronesSwitch : MonoBehaviour
{
    private GameObject[] drones;
    
    // Start is called before the first frame update
    void Start()
    {
        drones = GameObject.FindGameObjectsWithTag("Drone");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            foreach (var drone in drones)
            {
                drone.GetComponent<DroneController>().isActivated = false;
            }
            // Level clear, return to menu
            Debug.Log("Level Clear");
        }
    }
}
