using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.AI;
using UnityEditor;
using UnityEditor.AI;
using Unity.VisualScripting;
using System.Security.Principal;


[ExecuteInEditMode]

public class SpawnPrefabObjects : MonoBehaviour
{
    public List<GameObject> spawnObjects;
    public bool execute;
    public BoxCollider spawnArea;
    private GameObject parent;

    public float spawnThreshold = 0.3f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (execute)
        {
            spawnItems();
            execute = false;
        }
    }
    void spawnItems()
    {
        parent = this.gameObject;
        Bounds bounds = spawnArea.bounds;
        foreach (GameObject item in spawnObjects)
        {
            float spawnFloat = Random.Range(0f, 1f);

            if (spawnFloat <= spawnThreshold)
            {
                float offsetX = Random.Range(-bounds.extents.x, bounds.extents.x);
                // float offsetY = Random.Range(-bounds.extents.y, bounds.extents.y);
                float offsetZ = Random.Range(-bounds.extents.z, bounds.extents.z);

                Vector3 spawnPosition = bounds.center + new Vector3(offsetX, 0, offsetZ);

                GameObject spawnedObject = GameObject.Instantiate(item, spawnPosition, Quaternion.Euler(Quaternion.identity.x, Random.Range(0,360), Quaternion.identity.z));

                spawnedObject.transform.parent = parent.transform;
            }
        }
    }
}
