using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestColourChange : MonoBehaviour
{
    private Material material;
    private string emmisionID = "_EmissionColour";

    public Color green;
    public Color yellow;
    public Color red;

    // Start is called before the first frame update
    void Start()
    {
        material= GetComponent<SkinnedMeshRenderer>().sharedMaterial;
        material.SetColor(emmisionID, green);
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.Z))
        {
            material.SetColor(emmisionID, green);
        }
        if (Input.GetKey(KeyCode.X))
        {
            material.SetColor(emmisionID, yellow);
        }
        if (Input.GetKey(KeyCode.C))
        {
            material.SetColor(emmisionID, red);
        }
    }
}
