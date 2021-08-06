using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    private int mapWidth;
    [SerializeField]
    private int mapHeight;
    [SerializeField]
    private float noiseScale;
    private DisplayMap DisplayMapReference;
    public bool autoUpdateMap;
    [SerializeField]
    private float x;
    [SerializeField]
    private float y;


    public void GenerateMap()
    {
        float[,] noiceMap = NoiseGenerator.GenerateNoiceMap(mapWidth, mapHeight, noiseScale, x, y);
        DisplayMapReference = FindObjectOfType<DisplayMap>();
        DisplayMapReference.DrawMap(noiceMap);
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
