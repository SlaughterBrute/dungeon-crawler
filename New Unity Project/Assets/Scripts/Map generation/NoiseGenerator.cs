using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NoiseGenerator
{
    public static float[,] GenerateNoiceMap(int mapWidth, int mapHeight, float scale, int octaves, float persistance, float lacunarity, string seed, float xpos, float ypos)
    {   //octaves general shape
        //lacunarity small details in octaves
        //persistance lacunaritys influence on octaves
        float[,] map = new float[mapWidth, mapHeight];
        if (scale <= 0)
        {
            scale = 0.0001f;
        }

        //seed
        int seedNumber = seed.GetHashCode();
        Vector2[] positionOffcet = new Vector2[octaves];
        System.Random randGen = new System.Random(seedNumber);
        for(int i = 0; i < octaves; i++)
        {
            positionOffcet[i].x = randGen.Next(-80000, 80000) + xpos;
            positionOffcet[i].y = randGen.Next(-80000, 80000) + ypos;
        }

        //used to correct zooming and scaling the noice
        float halfMapWidth = mapWidth / 2f;
        float halfMapHeight = mapHeight / 2f;

        float maxHeight = float.MinValue;
        float minHeight = float.MaxValue;

        for(int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0; 

                for(int i = 0; i < octaves; i++)
                {
                    float sampleX = (x-halfMapWidth) / scale * frequency;
                    sampleX += positionOffcet[i].x * Mathf.Pow(lacunarity, i);
                    float sampleY = (y-halfMapHeight) / scale * frequency;
                    sampleY += positionOffcet[i].y * Mathf.Pow(lacunarity, i);

                    //get perlin value
                    float perlinValue = (Mathf.PerlinNoise(sampleX, sampleY) * 2) - 1;
                    noiseHeight += perlinValue * amplitude;
                    //prep for next octave
                    amplitude *= persistance;
                    frequency *= lacunarity;

                    
                }
                if(noiseHeight < minHeight)
                {
                    minHeight = noiseHeight;
                }
                else if(noiseHeight > maxHeight)
                {
                    maxHeight = noiseHeight;
                }
                map[x, y] = noiseHeight;
            }
        }

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                map[x, y] = Mathf.InverseLerp(minHeight, maxHeight, map[x, y]);
            }
        }
        
                return map;
    }

    public static float[,] GenerateNoiceMapForChunk()
    {
        float[,] map = { { 1f, 2f } };
        return map;
    }
}
