using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private int mapWidth;
    [SerializeField] private int mapHeight;
    [SerializeField] private float noiseScale;
    private DisplayMap DisplayMapReference;
    public bool autoUpdateMap;
    [SerializeField] private float x;
    [SerializeField] private float y;
    [SerializeField][Range(1,5)] private int octaves;
    [SerializeField][Range(0,1)] private float persistance;
    [SerializeField] private float lacunarity; // Detail for each octave
    [SerializeField] private string seed;

    public void OnValidate()
    {
        if (mapWidth < 1)
        {
            mapWidth = 1;
        }
        if(mapHeight < 1)
        {
            mapHeight = 1;
        }
        if(octaves < 1)
        {
            octaves = 1;
        }
    }

    public void GenerateMap()
    {
        float[,] noiceMap = NoiseGenerator.GenerateNoiceMap(mapWidth, mapHeight, noiseScale, octaves, persistance, lacunarity, seed, x, y);
        DisplayMapReference = FindObjectOfType<DisplayMap>();
        DisplayMapReference.DrawMap(noiceMap);
    }

    private void Update()
    {
        GenerateMap();
        x += 1 * Time.deltaTime;
    }
}
