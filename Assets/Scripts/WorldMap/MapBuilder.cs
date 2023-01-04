using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.AI;
using UnityEditor;
using UnityEditor.AI;

[ExecuteInEditMode]
public class MapBuilder : MonoBehaviour
{
    public int blockSize = 3;
    public bool execute;
    private GameObject parent;
    public Texture2D WorldTexture;
    public GameObject GrassCube;
    public GameObject DeepGrassCube;
    public GameObject DyingGrassCube;
    public GameObject DeadCube;
    public GameObject Bridge;
    public GameObject TowerCube;
    // public  GameObject WaterCube;
    public GameObject SciFiBridge;
    Dictionary<float, GameObject> TileDict = new Dictionary<float, GameObject>();

    private void Update()
    {
        if (execute)
        {
            TileDict.Clear();
            // WorldList.Clear();
            sceneSetup();
            execute = false;
        }
    }

    public void sceneSetup()
    {
        if (parent != null)
        {
            DestroyImmediate(parent);
        }
        parent = new GameObject("Parent");

        TileDict.Add(1.0f, GrassCube);
        TileDict.Add(0.9f, DeepGrassCube);
        TileDict.Add(0.8f, DyingGrassCube);
        TileDict.Add(0.7f, DeadCube);
        TileDict.Add(0.6f, Bridge);
        TileDict.Add(0.5f, TowerCube);
        // TileDict.Add(0.0f, WaterCube);
        TileDict.Add(0.4f, SciFiBridge);

        for (int i = 0; i < WorldTexture.height; i++)
        {
            // List<float> texRow = new List<float>();
            for (int j = 0; j < WorldTexture.width; j++)
            {
                float pixel_g = WorldTexture.GetPixel(i, j).g;
                // float pixel_g = Mathf.Round(pixel.g);
                pixel_g = Mathf.Round(pixel_g * 10f) / 10f;
                // texRow.Add(pixel_g);
                if (pixel_g != 0.0f)
                {
                    GameObject Tile = Instantiate(
                        TileDict[pixel_g], new Vector3(
                            i * blockSize, 0, j * blockSize
                        ), Quaternion.identity
                    );
                    Tile.transform.parent = parent.transform;

                    if (pixel_g != 0.6f) // Random selection of 90 degree rotations
                    {
                        int randomRotator = Random.Range(1, 3);
                        Tile.transform.eulerAngles = new Vector3(
                            parent.transform.eulerAngles.x,
                            parent.transform.eulerAngles.y + 90 * randomRotator,
                            parent.transform.eulerAngles.z
                            );
                    }

                    // Set static navigation
                    var navFlag = StaticEditorFlags.NavigationStatic;
                    GameObjectUtility.SetStaticEditorFlags(Tile, navFlag);
                    // Add box collider for each tile
                    // Tile.AddComponent<BoxCollider>();
                }
            }
        }
        // NavMeshBuilder.ClearAllNavMeshes();
        // NavMeshBuilder.BuildNavMesh();
    }
}