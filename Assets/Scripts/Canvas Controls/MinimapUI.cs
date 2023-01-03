using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapUI : MonoBehaviour
{
    public Transform masterSwitch;
    public Camera minimapCam;
    public Transform playerPosition;
    public Transform playerRoot;
    public RectTransform pointer;
    public RectTransform minimapGUI;
    private void Start()
    {
        playerPosition = GameObject.Find("MinimapIcon").transform;
        playerRoot = GameObject.Find("Main Camera").transform;
        masterSwitch = GameObject.Find("Master Switch").transform;
        minimapCam = GameObject.Find("MinimapCam").GetComponent<Camera>();
    }
    void Update()
    {
        //For Master Switch
        Vector3 offset = Vector3.ClampMagnitude(masterSwitch.position - playerPosition.position, minimapCam.orthographicSize) / minimapCam.orthographicSize * (minimapGUI.rect.width / 2f);
        Vector3 rotatedOffset = playerRoot.forward * offset.x + playerRoot.right * offset.z;
        pointer.anchoredPosition = new Vector2(rotatedOffset.z, rotatedOffset.x);

    }
}