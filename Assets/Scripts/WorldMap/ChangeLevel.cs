using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLevel : MonoBehaviour
{
    private Light towerLight;
    public GAME_STATE newState;
    private bool isPlayerOverlap;

    private void Start()
    {
        towerLight = GetComponent<Light>();    
    }

    private void illuminate()
    {
        towerLight.intensity = 50;
    }

    private void delluminate()
    {
        towerLight.intensity = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            isPlayerOverlap = true;
            illuminate();
            // Debug.Log(string.Format("Player enter {0}", isPlayerOverlap));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            isPlayerOverlap = false;
            delluminate();
            // Debug.Log(string.Format("Player enter {0}", isPlayerOverlap));
        }
    }

    

    private void OnMouseDown()
    {
        if (isPlayerOverlap)
        {
            GameManager.UpdateGameState(newState);
        }
    }
}
