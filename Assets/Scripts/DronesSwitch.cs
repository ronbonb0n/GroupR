using UnityEngine;

public class DronesSwitch : MonoBehaviour
{
    private GameObject[] drones;
    public LevelCanvasControls CanvasControls;
    public GameObject Salvage;
    
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
                drone.GetComponentInParent<DroneController>().isActivated = false;
                Debug.Log(Instantiate(Salvage, new Vector3(drone.transform.position.x, drone.transform.position.y+2, drone.transform.position.z), drone.transform.rotation));
            }
            LevelClear();
        }
    }

    private void LevelClear()
    {
        CanvasControls.onLevelWon();
        GameManager.UpdateGameState((GAME_STATE) GameManager.State + 1);
    }
}