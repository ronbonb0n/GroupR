using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLevel : MonoBehaviour
{
    private Light towerLight;
    public LEVELS Level;
    private bool isPlayerOverlap;
    private Transform playerTransform;

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
            playerTransform = other.transform;
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
            if (GameManager.CheckNextLevel(Level))
            {
                GameManager.playerInWorldMap = playerTransform.position;
                GameManager.SwitchLevel(Level);
            }
        }
    }
}
