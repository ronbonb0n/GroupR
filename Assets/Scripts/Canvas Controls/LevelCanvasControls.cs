using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelCanvasControls : MonoBehaviour
{
    public GameObject LevelWonText;
    public GameObject LevelLostText;
    public bool levelOver;
    public TextMeshProUGUI[] InventoryTexts;
    void Start()
    {     
        LevelLostText.SetActive(false);
        LevelWonText.SetActive(false);
        levelOver = false;
        onItemUse();
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
        GameManager.SwitchLevel(LEVELS.WORLD_MAP);
    }
    public void onRetry()
    {
        GameManager.SwitchLevel(GameManager.Level); // all LEVEL_LOST states are LEVEL_START states + 1 
    }
    public void onItemUse()
    {
        for(var i=0; i<InventoryManager.Inventory.Length; i++)
        {
            InventoryTexts[i].text = InventoryManager.Inventory[i].ToString();
        }
    }
}
