using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLevel : MonoBehaviour
{
    public LEVELS newLevel;
    private bool isPlayerOverlap;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            isPlayerOverlap = true;
            // Debug.Log(string.Format("Player enter {0}", isPlayerOverlap));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            isPlayerOverlap = false;
            // Debug.Log(string.Format("Player enter {0}", isPlayerOverlap));
        }
    }

    private void OnMouseDown()
    {
        if (isPlayerOverlap)
        {
            GameManager.SwitchLevel(newLevel);
            //TO DO: Change GameState after level change
        }
    }
}
