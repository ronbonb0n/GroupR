using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TowerGUI : MonoBehaviour
{
    private Transform cameraTransform;
    public TextMeshPro LevelText;
    public Color completeLevelColor = new Color(0.5981132f, 0.964274f, 1, 1);
    public Color currentLevelColor = new Color(0.5981132f, 0.964274f, 0, 1);
    public Color incompleteLevelColor = new Color(0.5f , 0, 0, 1);
    public LEVELS levelNumber;


    private void Start()
    {
        cameraTransform = GameObject.Find("Camera").transform;
        LevelTextColor();
    }

    
    private void Update()
    {
        transform.LookAt(cameraTransform);
    }
    void LevelTextColor()
    {
        if (levelNumber == LEVELS.VENDOR)
        {
            LevelText.color = completeLevelColor;
            return;
        }
        if ((int)GameManager.State > (int) levelNumber-1)
        {
            LevelText.color = completeLevelColor;
        }
        else if ((int)GameManager.State == (int)levelNumber-1)
        {
            LevelText.color = currentLevelColor;
        }
        else
        {
            LevelText.color = incompleteLevelColor;
        }
    }
}
