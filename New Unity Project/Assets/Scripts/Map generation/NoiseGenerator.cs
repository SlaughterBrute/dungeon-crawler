using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NoiseGenerator
{
    public static float[,] GenerateNoiceMap(int mapWidth, int mapHeight, float scale, float xpos, float ypos)
    {
        float[,] map = new float[mapWidth, mapHeight];
        if (scale <= 0)
        {
            scale = 0.0001f;
        }

        for(int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                float sampleX = x / scale;
                sampleX += xpos;
                float sampleY = y / scale;
                sampleY += ypos;

                float perlinValue = Mathf.PerlinNoise(sampleX, sampleY);
                map[x, y] = perlinValue;
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
