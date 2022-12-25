using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCanvasControls : MonoBehaviour
{
    public GameObject LevelWonText;
    public GameObject LevelLostText;
    public bool levelOver;
    void Start()
    {     
        LevelLostText.SetActive(false);
        LevelWonText.SetActive(false);
        levelOver = false;
    }

    public void onLevelLost()
    {
        if (!levelOver)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            LevelLostText.SetActive(true);
            levelOver = true;
        }
    }

    public void onLevelWon()
    {
        if (!levelOver)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            LevelWonText.SetActive(true);
            levelOver = false;
        }
    }

    public void onReturnToWorldMap()
    {
        GameManager.UpdateGameState(GameManager.State + 2); // all LEVEL_WON states are LEVEL_START states + 2
    }
    public void onRetry()
    {
        GameManager.UpdateGameState(GameManager.State + 1); // all LEVEL_LOST states are LEVEL_START states + 1 
    }
}
