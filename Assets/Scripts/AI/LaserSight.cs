using System;
using System.Collections;
using UnityEngine;

public class LaserSight : MonoBehaviour
{
    public float fov = 90f;
    public int sliceCount = 20;
    private float angleIncrease;
    private float viewDistance;
    
    private Vector3[] vertices;
    private Vector2[] uv;
    private int[] indices;
    
    private Mesh mesh;

    private float laserPitch;

    // Start is called before the first frame update
    void Start()
    {
        FieldOfView fieldOfView = GetComponentInParent<FieldOfView>();
        viewDistance = fieldOfView.radius;
        laserPitch = (float) Math.Asin(transform.position.y / fieldOfView.radius);

        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        angleIncrease = fov / sliceCount;
        
        vertices = new Vector3[sliceCount + 2];
        uv = new Vector2[vertices.Length];
        indices = new int[sliceCount * 3];
        
        UpdateLaserSight();
        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 0; i <= sliceCount; i++)
        {
            if (i >= 1)
            {
                indices[triangleIndex] = 0;
                indices[triangleIndex + 1] = vertexIndex - 1;
                indices[triangleIndex + 2] = vertexIndex;
                triangleIndex += 3;
            }
            vertexIndex++;
        }
        mesh.uv = uv;
        mesh.triangles = indices;

        StartCoroutine(LaserSightRoutine());
    }

    private Vector3 RotateVector(float angle)
    {
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad) - Mathf.Sin(angleRad), (float) -Math.Sin(laserPitch), Mathf.Sin(angleRad) + Mathf.Cos(angleRad));
    }
    
    private IEnumerator LaserSightRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(1f);

        while (true)
        {
            yield return wait;
            UpdateLaserSight();
        }
    }
    
    void UpdateLaserSight()
    {
        float angle = 0;

        vertices[0] = Vector3.zero;
        int vertexIndex = 1;
        for (int i = 0; i <= sliceCount; i++)
        {
            Vector3 vertex = Vector3.Normalize(RotateVector(angle)) * viewDistance;
            
            // RaycastHit hit;
            // Vector3 destination = Quaternion.AngleAxis(transform.eulerAngles.y, Vector3.up) * vertex;
            // if (Physics.Raycast(transform.position, destination, out hit, viewDistance))
            // {
            //     Debug.DrawLine(transform.position, hit.point, Color.white, 0.01f);
            //     vertex = RotateVector(angle) * hit.distance;
            // }
            
            vertices[vertexIndex] = vertex;
            angle += angleIncrease;
            vertexIndex++;
        }

        mesh.vertices = vertices;
    }
}
