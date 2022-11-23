using System;
using System.Collections;
using UnityEngine;

// reference: Field of View Effect in Unity (Line of Sight, View Cone) https://www.youtube.com/watch?v=CSeUMTaNFYk
public class LaserSight : MonoBehaviour
{
    public float fov = 90f;
    public int sliceCount = 20;
    private float angleIncrease;
    private float viewDistance;
    
    private Vector3[] standardVertices;
    private Vector2[] uv;
    private int[] indices;
    
    private Mesh mesh;

    public float laserPitch;
    public float laserPitchRad;

    // Start is called before the first frame update
    void Start()
    {
        FieldOfView fieldOfView = GetComponentInParent<FieldOfView>();
        viewDistance = fieldOfView.radius;
        Debug.Log("Drone - Sight Radius:  " + viewDistance);
        laserPitch = transform.position.y / fieldOfView.radius;
        laserPitchRad = (float)(Math.Asin(laserPitch) * 180 / Math.PI);
        var lines = transform.parent.GetComponentsInChildren<LineRenderer>();
        foreach (var line in lines)
        {
            line.transform.Rotate(laserPitchRad, 0, 0);
        }

        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        angleIncrease = fov / sliceCount;
        
        standardVertices = new Vector3[sliceCount + 2];
        uv = new Vector2[standardVertices.Length];
        indices = new int[sliceCount * 3];
        
        float angle = 0;

        standardVertices[0] = Vector3.zero;
        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 0; i <= sliceCount; i++)
        {
            Vector3 vertex = RotateVector(angle);
            standardVertices[vertexIndex] = vertex;
            angle += angleIncrease;
            if (i >= 1)
            {
                indices[triangleIndex] = 0;
                indices[triangleIndex + 1] = vertexIndex - 1;
                indices[triangleIndex + 2] = vertexIndex;
                triangleIndex += 3;
            }
            vertexIndex++;
        }
        
        mesh.vertices = standardVertices;
        mesh.uv = uv;
        mesh.triangles = indices;

        // StartCoroutine(LaserSightRoutine());
    }

    private Vector3 RotateVector(float angle)
    {
        float angleRad = angle * (Mathf.PI / 180f);
        
        Vector3 baseVector = new Vector3(Mathf.Sqrt(2) / 2, 0, Mathf.Sqrt(2) / 2);
        Vector3 yaw = new Vector3(Mathf.Cos(angleRad) * baseVector.x - Mathf.Sin(angleRad) * baseVector.z,
            0,
            Mathf.Sin(angleRad) * baseVector.x + Mathf.Cos(angleRad) * baseVector.z);
        Vector3 ret = Quaternion.AngleAxis(laserPitchRad, Vector3.right) * yaw * viewDistance;
        
        // Debug.Log(ret.magnitude);
        // Debug.DrawLine(transform.position, transform.position + ret, Color.gray, 0.1f);
        
        return ret;
    }
    
    private IEnumerator LaserSightRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.01f);

        while (true)
        {
            yield return wait;
            UpdateLaserSight();
        }
    }
    
    void UpdateLaserSight()
    {
        Vector3[] vertices = new Vector3[sliceCount + 2];
        vertices[0] = Vector3.zero;
        for (int i = 1; i <= sliceCount; i++)
        {
            vertices[i] = standardVertices[i];
            // RaycastHit hit;
            // if (Physics.Raycast(transform.position, vertices[i], out hit, viewDistance))
            // {
            //     Debug.DrawLine(transform.position, hit.point, Color.white, 0.01f);
            //     vertices[i] = hit.point - transform.position;
            // }
        }

        mesh.vertices = vertices;
    }
}
