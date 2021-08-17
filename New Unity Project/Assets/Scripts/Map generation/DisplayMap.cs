using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayMap : MonoBehaviour
{
    [SerializeField]
    private Renderer textureRenderer;
    public void DrawMap(float[,] noiceMap, TerrainType[] terrainTypes)
    {
        int width = noiceMap.GetLength(0);
        int height = noiceMap.GetLength(1);

        Texture2D texture = new Texture2D(width, height);
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.filterMode = FilterMode.Point;
        Color [] colors = new Color[width * height];
        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                if(terrainTypes.Length == 0 || terrainTypes == null) //withour color
                {
                    colors[x + y * width] = Color.Lerp(Color.white, Color.black, noiceMap[x, y]);
                }
                else //with color
                {
                    for(int i = 0; i < terrainTypes.Length;i++)
                    {
                        if (noiceMap[x, y] <= terrainTypes[i].height)
                        {
                            colors[x + y * width] = terrainTypes[i].terrainColor;
                            break;
                        }      
                    }
                }
            }
        }

        texture.SetPixels(colors);
        texture.Apply();

        textureRenderer.sharedMaterial.mainTexture = texture;
        textureRenderer.transform.localScale = new Vector3(width, 1, height);
    }
}
