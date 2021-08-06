using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayMap : MonoBehaviour
{
    [SerializeField]
    private Renderer textureRenderer;
    public void DrawMap(float[,] noiceMap)
    {
        int width = noiceMap.GetLength(0);
        int height = noiceMap.GetLength(1);

        Texture2D texture = new Texture2D(width, height);
        Color [] colors = new Color[width * height];
        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                colors[x + y * width] = Color.Lerp(Color.white, Color.black, noiceMap[x, y]);
            }
        }
        texture.SetPixels(colors);
        texture.Apply();

        textureRenderer.sharedMaterial.mainTexture = texture;
        textureRenderer.transform.localScale = new Vector3(width, 1, height);
    }
}
