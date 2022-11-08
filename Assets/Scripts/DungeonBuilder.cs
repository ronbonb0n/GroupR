using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.AI;
using UnityEditor;
//using UnityEditor.AI;

[ExecuteInEditMode]
public class DungeonBuilder : MonoBehaviour
{
    [Header("Consistent tile width and length")] // Currently only supports square blocks
    public int blockSize = 6;

    [Header("Dungeon Bitmap")]
    public Texture2D dungeonTexture;

    [Header("Open Floor Prefab")]
    public  GameObject openArea;

    [Header("Outer Corner Prefabs")]
    public  GameObject topRightOuterCorner;
    public  GameObject topLeftOuterCorner;
    public  GameObject bottomRightOuterCorner;
    public  GameObject bottomLeftOuterCorner;

    [Header("Inner Corner Prefabs")]
    public  GameObject topRightInnerCorner;
    public  GameObject topLeftInnerCorner;
    public  GameObject bottomRightInnerCorner;
    public  GameObject bottomLeftInnerCorner;

    [Header("Wall Prefabs")]
    public  GameObject leftWall;
    public  GameObject rightWall;
    public  GameObject topWall;
    public  GameObject bottomWall;

    [Header("Corridor Prefabs")]
    public GameObject upDownCorridor;
    public GameObject acrossCorridor;

    [Header("Doorway Prefabs")]
    public  GameObject leftDoorway;
    public  GameObject rightDoorway;
    public  GameObject topDoorway;
    public  GameObject bottomDoorway;

    [Header("Build level")]
    public bool execute;

    private GameObject parent;
    // Tuple for RGB data of each pixel
    (float, float, float) pixelData;
    // 2D list to contain entire image data
    List<List<(float, float, float)>> dungeonData = new List<List<(float, float, float)>>();
    Dictionary<(float, float, float), GameObject> dungeonTileDict = new Dictionary<(float, float, float), GameObject>();
    
    private void Update() {
        if (execute)
        {
            buildLookup();
            buildDungeonArray();
            buildScene();
            execute = false;
        }
    }

    public void buildScene()
    {   
        if (parent != null)
        {
            DestroyImmediate(parent);
        }
        parent = new GameObject("Parent");

        int row = 0;
        int col = 0;
        foreach (var sceneRow in dungeonData)
        {
            foreach (var pixel in sceneRow)
            {
                if (pixel != (0f, 0f, 0f))
                {
                    GameObject Tile = Instantiate(
                        dungeonTileDict[pixel], new Vector3(
                            row * blockSize, 0, col * blockSize
                        ), Quaternion.identity
                    );
                    Tile.transform.parent = parent.transform;
                    // Set static navigation
                    var navFlag = StaticEditorFlags.NavigationStatic;
                    GameObjectUtility.SetStaticEditorFlags(Tile, navFlag);
                }
                col += 1;
                if (col >= sceneRow.Count)
                {
                    col = 0;
                }
            }
            row += 1;
            if (row >= dungeonData.Count)
            {
                row = 0;
            }
        }
    }

    public void buildLookup ()
    {
        // clear old data
        dungeonTileDict.Clear();

        // Populate tile lookup
        // open area
        dungeonTileDict.Add((1.0f, 1.0f, 1.0f), openArea);
        // outer corners
        dungeonTileDict.Add((0.2f, 0.4f, 0.3f), topRightOuterCorner);
        dungeonTileDict.Add((0.0f, 1.0f, 1.0f), topLeftOuterCorner);
        dungeonTileDict.Add((0.3f, 1.0f, 0.4f), bottomRightOuterCorner);
        dungeonTileDict.Add((1.0f, 1.0f, 0.0f), bottomLeftOuterCorner);
        // inner corners
        dungeonTileDict.Add((1.0f, 0.7f, 1.0f), topRightInnerCorner);
        dungeonTileDict.Add((0.8f, 1.0f, 0.5f), topLeftInnerCorner);
        dungeonTileDict.Add((1.0f, 0.5f, 0.6f), bottomRightInnerCorner);
        dungeonTileDict.Add((1.0f, 0.7f, 0.0f), bottomLeftInnerCorner);
        // walls
        dungeonTileDict.Add((0.0f, 0.0f, 1.0f), leftWall);
        dungeonTileDict.Add((0.0f, 1.0f, 0.0f), rightWall);
        dungeonTileDict.Add((0.5f, 0.6f, 1.0f), topWall);
        dungeonTileDict.Add((0.2f, 0.5f, 0.2f), bottomWall);
        // corridors
        dungeonTileDict.Add((0.2f, 0.3f, 0.5f), upDownCorridor);
        dungeonTileDict.Add((1.0f, 0.6f, 0.3f), acrossCorridor);
        // doorways
        dungeonTileDict.Add((0.3f, 0.3f, 0.3f), leftDoorway);
        dungeonTileDict.Add((0.7f, 0.7f, 0.7f), rightDoorway);
        dungeonTileDict.Add((1.0f, 0.0f, 0.3f), topDoorway);
        dungeonTileDict.Add((1.0f, 0.3f, 0.0f), bottomDoorway);
    }

    public void buildDungeonArray()
    {
        dungeonData.Clear();

        for (int i = 0; i < dungeonTexture.width; i++)
        {
            // List for each row of pixels - not sure why it has to be made new and not just cleared per iteration
            List<(float, float, float)> pixelRow = new List<(float, float, float)>();

            for (int j = 0; j < dungeonTexture.height; j++)
            {   
                pixelData = (
                    Mathf.Round(dungeonTexture.GetPixel(i, j).r * 10f) / 10f,
                    Mathf.Round(dungeonTexture.GetPixel(i, j).g * 10f) / 10f,
                    Mathf.Round(dungeonTexture.GetPixel(i, j).b * 10f) / 10f
                );

                pixelRow.Add(pixelData);
            }
            dungeonData.Add(pixelRow);
        }

    }
}
