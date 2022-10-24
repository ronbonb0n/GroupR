using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeLevel : MonoBehaviour
{
    public string newLevel;
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
            SceneManager.LoadScene(newLevel);
        }
    }
}
