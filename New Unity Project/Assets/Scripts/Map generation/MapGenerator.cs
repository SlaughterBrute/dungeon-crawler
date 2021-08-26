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
        float[,] noiseMap = returnNoiseMap(x, y);
        //float[,] noiseMap = NoiseGenerator.GenerateNoiseMap(chunkSize, chunkSize, noiseScale, octaves, persistance, lacunarity, seed, x, y);
        if (createTiledMap)
        {
            GenerateTiledCave(noiseMap);
        }
        DisplayMapReference = FindObjectOfType<DisplayMap>();
        DisplayMapReference.DrawMap(noiseMap, terrainTypes);
    }

    private float[,] returnNoiseMap(float x, float y)
    {
        float[,] noiseMap = NoiseGenerator.GenerateNoiseMap(chunkSize, chunkSize, noiseScale, octaves, persistance, lacunarity, seed, x, y);
        return noiseMap;
    }

    private void Update()
    {

    }

    public void PopulateTilemapWithTiles(Tilemap[] tilemaps, Vector2 chunkCoord)
    {
        //tilemap index
        //0: floor
        //1: wall
        //2: shadow

        CompositeCollider2D composite = tilemaps[1].GetComponent<CompositeCollider2D>();
        if (composite != null)
        {
            Random.InitState(123);
            tilemaps[0].ClearAllTiles();
            tilemaps[1].ClearAllTiles();
            tilemaps[2].ClearAllTiles();

            float[,] noiseMap = returnNoiseMap(chunkCoord.x * chunkSize, chunkCoord.y * chunkSize);
            for (int x = 0; x < chunkSize; x++)
            {
                for (int y = 0; y < chunkSize; y++)
                {
                    Vector3Int position = new Vector3Int(x, y, 0);
                    if (noiseMap[x, y] > 0.4)
                    {
                        tilemaps[0].SetTile(position, groudTile);
                        tilemaps[2].SetTile(position, floorShadowTile);
                    }
                    else
                    {
                        tilemaps[1].SetTile(position, wallTile);
                    }
                }
            }
            // need to generate colliders at the end of the frame, won't work otherwise. Probably a bug
            StartCoroutine(GenerateCollitionGeometry(composite));
        }
        else
        {
            Debug.LogError("Mapgenerator could not find tilemapWall's composite collider 2d");
        }

    }

    public IEnumerator GenerateCollitionGeometry(CompositeCollider2D cc2d)
    {
        if (cc2d != null)
        {
            yield return new WaitForEndOfFrame();
            cc2d.GenerateGeometry();
        }
    }

    //old system, populate predefined chunk with tiles
    private void GenerateTiledCave(float[,] noiseMap)
    {
        CompositeCollider2D composite = tilemapWall.GetComponent<CompositeCollider2D>();
        if (composite != null)
        {
            composite.generationType = CompositeCollider2D.GenerationType.Manual;
        }
        else
        {
            Debug.LogError("Mapgenerator could not find tilemapWall's composite collider 2d");
        }

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
                if (noiseMap[x, y] > 0.4)
                {
                    tilemapGround.SetTile(position, groudTile);
                    tilemapFloorShadow.SetTile(position, floorShadowTile);
                }
                else
                {
                    tilemapWall.SetTile(position, wallTile);
                }
            }
        }

        
        if (composite != null)
        {
            composite.GenerateGeometry();
        }
       
    }

    public int getChunkSize()
    {
        return chunkSize;
    }
}




[System.Serializable]
public struct TerrainType{
    public string terrainName;
    public float height;
    public Color terrainColor;
}
