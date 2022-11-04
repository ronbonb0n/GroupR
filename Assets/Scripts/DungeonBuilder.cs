using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.AI;
using UnityEditor;
using UnityEditor.AI;

[ExecuteInEditMode]
public class DungeonBuilder : MonoBehaviour
{
    public int blockSize = 6;
    public bool execute;

    // Tuple for RGB data of each pixel
    (float, float, float) pixelData;

    // 2D list to contain entire image data
    List<List<(float, float, float)>> dungeonData = new List<List<(float, float, float)>>();

    public Texture2D dungeonTexture;
    // list<> dungeonList
    // private GameObject parent;

    // [Header("Wall Sections")]
    // public  GameObject leftWall;
    // public  GameObject rightWall;
    // public  GameObject topWall;
    // public  GameObject bottomWall;

    // []
    // public  GameObject Bridge;
    // public  GameObject TowerCube;
    // // public  GameObject WaterCube;
    // public  GameObject SciFiBridge;
    // Dictionary<float, GameObject> TileDict = new Dictionary<float, GameObject>();
    
    private void Update() {
        if (execute)
        {
            // TileDict.Clear();
            dungeonData.Clear();
            sceneSetup();
            execute = false;
        }
    }

    public void sceneSetup()
    {
        
        // if (parent != null)
        // {
        //     DestroyImmediate(parent);
        // }
        // parent = new GameObject("Parent");

        // TileDict.Add(1.0f, GrassCube);
        // TileDict.Add(0.9f, DeepGrassCube);
        // TileDict.Add(0.8f, DyingGrassCube);
        // TileDict.Add(0.7f, DeadCube);
        // TileDict.Add(0.6f, Bridge);
        // TileDict.Add(0.5f, TowerCube);
        // // TileDict.Add(0.0f, WaterCube);
        // TileDict.Add(0.4f, SciFiBridge);
        
        for (int i = 0; i < dungeonTexture.width; i++)
        {
            // List for each row of pixels - not sure why it has to be made new and not just cleared
            List<(float, float, float)> pixelRow = new List<(float, float, float)>();

            for (int j = 0; j < dungeonTexture.height; j++)
            {   
                pixelData = (
                    Mathf.Round(dungeonTexture.GetPixel(i, j).r * 10f) / 10f,
                    Mathf.Round(dungeonTexture.GetPixel(i, j).g * 10f) / 10f,
                    Mathf.Round(dungeonTexture.GetPixel(i, j).b * 10f) / 10f
                );
                // Debug.Log(pixelData);

                pixelRow.Add(pixelData);
                // if (pixel_g != 0.0f)
                // {
                //     GameObject Tile = Instantiate(
                //         TileDict[pixel_g], new Vector3(
                //             i * blockSize, 0 , j * blockSize
                //         ), Quaternion.identity
                //     );
                //     Tile.transform.parent = parent.transform;
                //     // Set static navigation
                //     var navFlag = StaticEditorFlags.NavigationStatic;
                //     GameObjectUtility.SetStaticEditorFlags(Tile, navFlag);
                //     // Add box collider for each tile
                //     Tile.AddComponent<BoxCollider>();
                // }
            }
            dungeonData.Add(pixelRow);
        }
        // NavMeshBuilder.ClearAllNavMeshes();
        // NavMeshBuilder.BuildNavMesh();

        for (int i = 0; i < dungeonData.Count; i++)
        {
            Debug.Log(string.Join(", ", dungeonData[i]));
        }

    }
}
