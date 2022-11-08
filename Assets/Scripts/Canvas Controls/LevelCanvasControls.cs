using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCanvasControls : MonoBehaviour
{
    public GameObject LevelWonText;
    public GameObject LevelLostText;
    public bool ShowCursor = false; // TESTING VAR DELETE WHEN DONE
    // Start is called before the first frame update
    void Start()
    {     
            LevelLostText.SetActive(false);
            LevelWonText.SetActive(false);
    }

    private void Update() // TESTING FUNCTION DELETE WHEN DONE
    {
        Cursor.visible = ShowCursor;
        Cursor.lockState = ShowCursor?CursorLockMode.Confined:CursorLockMode.Locked;
    }
    public void onLevelLost()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        LevelLostText.SetActive(true);
    }

    public void onLevelWon()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        LevelWonText.SetActive(true);
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
