using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRespectiveRotation : MonoBehaviour
{
    public GameObject player;
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 viewDir = player.transform.position - new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
        player.transform.forward = viewDir.normalized;
    }
}
