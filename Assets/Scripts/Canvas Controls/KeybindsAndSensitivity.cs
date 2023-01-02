using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;

public class KeybindsAndSensitivity : MonoBehaviour
{
    public CinemachineFreeLook CameraControl;
    public Slider cameraSensitivitySlider;
    public int maxSensitivity;
    public int minSensitivity;
    
    void Start()
    { 
        CameraControl = GameObject.Find("CameraControl").GetComponent<CinemachineFreeLook>();
        cameraSensitivitySlider.minValue = minSensitivity;
        cameraSensitivitySlider.maxValue = maxSensitivity;
        CameraControl.m_XAxis.m_MaxSpeed = minSensitivity + ((maxSensitivity-minSensitivity)/2);
        cameraSensitivitySlider.value = CameraControl.m_XAxis.m_MaxSpeed;
    }


    public void CameraSensitivity()
    {
        CameraControl.m_XAxis.m_MaxSpeed = cameraSensitivitySlider.value;
    }

}
