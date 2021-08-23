using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private int chunkSize;
    //[SerializeField] private int chunkSize;
    //[SerializeField] private int chunkSize;
    [SerializeField] private float noiseScale;
    private DisplayMap DisplayMapReference;
    public bool autoUpdateMap;
    [SerializeField] private float x;
    [SerializeField] private float y;
    [SerializeField] [Range(1, 5)] private int octaves;
    [SerializeField] [Range(0, 1)] private float persistance;
    [SerializeField] private float lacunarity; // Detail for each octave
    [SerializeField] private string seed;
    [SerializeField] private TerrainType[] terrainTypes;

    [SerializeField] private bool createTiledMap;
    [SerializeField] private Tilemap tilemapGround;
    [SerializeField] private Tilemap tilemapFloorShadow;
    [SerializeField] private Tilemap tilemapWall;
    [SerializeField] private Tile groudTile;
    [SerializeField] private Tile wallTile;
    [SerializeField] private Tile floorShadowTile;



    public void OnValidate()
    {
        if (chunkSize < 1)
        {
            chunkSize = 1;
        }
        if (octaves < 1)
        {
            octaves = 1;
        }
    }

    public void GenerateMap()
    {
        float[,] noiceMap = NoiseGenerator.GenerateNoiceMap(chunkSize, chunkSize, noiseScale, octaves, persistance, lacunarity, seed, x, y);
        if (createTiledMap)
        {
            GenerateTiledCave(noiceMap);
        }
        DisplayMapReference = FindObjectOfType<DisplayMap>();
        DisplayMapReference.DrawMap(noiceMap, terrainTypes);
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3Int pos = new Vector3Int(1, 0, 0);
            Debug.Log(tilemapWall.GetColliderType(pos));
            //Debug.Log(wallTile.colliderType);
        }
        //GenerateMap();
        //x += 1 * Time.deltaTime;
    }

    private void GenerateTiledCave(float[,] noiceMap)
    {
        Debug.Log("Build");
        Random.InitState(123);
        tilemapGround.ClearAllTiles();
        tilemapWall.ClearAllTiles();
        tilemapFloorShadow.ClearAllTiles();
        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);
                if (noiceMap[x, y] > 0.4)
                {
                    tilemapGround.SetTile(position, groudTile);
                    tilemapFloorShadow.SetTile(position, floorShadowTile);
                }
                else
                {
                    tilemapWall.SetTile(position, wallTile);
                    tilemapWall.SetColliderType(position, wallTile.colliderType);
                }
            }
        }

        CompositeCollider2D composite = tilemapWall.GetComponent<CompositeCollider2D>();
        if (composite != null)
        {
            composite.generationType = CompositeCollider2D.GenerationType.Manual;
            composite.GenerateGeometry();
        }
        else
        {
            Debug.LogError("Map generator could not find walltilemap's composite collider 2d");
        }
    }
}




[System.Serializable]
public struct TerrainType{
    public string terrainName;
    public float height;
    public Color terrainColor;
}
