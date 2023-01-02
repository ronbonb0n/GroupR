using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMapCanvasControls : MonoBehaviour
{
    public GameObject ControlsBtn;
    public GameObject ClearScreen;

    // Start is called before the first frame update
    void Start()
    {
        ControlsBtn.SetActive(true);
        ClearScreen.SetActive(false);   
    }
    public void OnToggle()
    {
        ControlsBtn.SetActive(!ControlsBtn.activeInHierarchy);
        ClearScreen.SetActive(!ClearScreen.activeInHierarchy);
    }
}
