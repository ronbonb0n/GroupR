using UnityEngine;

public class LaserSight : MonoBehaviour
{
    public float fov = 90f;
    public int sliceCount = 10;
    public float viewDistance = 10f;
    
    public LayerMask obstructionMask;

    // Start is called before the first frame update
    void Start()
    {
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        float angleIncrease = fov / sliceCount;
        float angle = 0;
        Vector3 origin = Vector3.zero;
        Vector3[] vertices = new Vector3[sliceCount + 2];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] indices = new int[sliceCount * 3];
        
        vertices[0] = Vector3.zero;
        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 0; i <= sliceCount; i++)
        {
            Vector3 vertex = origin + RotateVector(angle) * viewDistance;
            RaycastHit hit;
            if (Physics.Raycast(origin, RotateVector(angle), out hit, viewDistance))
            {
                vertex = hit.point;
            }
            vertices[vertexIndex] = vertex;
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

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = indices;
    }

    private Vector3 RotateVector(float angle)
    {
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad) - Mathf.Sin(angleRad), -0.5f, Mathf.Sin(angleRad) + Mathf.Cos(angleRad));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
