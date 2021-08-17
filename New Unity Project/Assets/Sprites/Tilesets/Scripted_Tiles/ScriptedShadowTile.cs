using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ScriptedShadowTile : Tile
{

    public Sprite[] m_Sprites;
    public Sprite m_Preview;


    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector3Int tilePosition = new Vector3Int(position.x + x, position.y + y, position.z);
                if (HasTile(tilemap, tilePosition))
                {
                    tilemap.RefreshTile(position);
                }
            }
        }
    }

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData) //need Get index and Get rotation
    {
        //base.GetTileData(position, tilemap, ref tileData);
        int mask = HasTile(tilemap, position + new Vector3Int(0, 1, 0)) ? 1 : 0;
        mask += HasTile(tilemap, position + new Vector3Int(1, 0, 0)) ? 2 : 0;
        mask += HasTile(tilemap, position + new Vector3Int(0, -1, 0)) ? 4 : 0;
        mask += HasTile(tilemap, position + new Vector3Int(-1, 0, 0)) ? 8 : 0;
        int index = GetIndex((byte)mask);
        if (index >= 0 && index < m_Sprites.Length)
        {
            tileData.sprite = m_Sprites[index];
            tileData.color = Color.white;
            var m = tileData.transform;
            m.SetTRS(Vector3.zero, GetRotation((byte)mask), Vector3.one);
            tileData.transform = m;
            tileData.flags = TileFlags.LockTransform;
            tileData.colliderType = ColliderType.None;
        }
        else
        {
            Debug.LogWarning("Tried to get non existing sprite");
        }
    }

    private int GetIndex(byte mask)
    {
        Debug.Log("Index: " + mask);
        switch (mask) // fixa senare beroende p� hur v�gg och mark f�rh�llande
        {
            case 15: return 0;

            case 7:
            case 11:
            case 13:
            case 14: return 1; // 1 shadow

            case 3: 
            case 6:
            case 9:
            case 12: return 2; // 2 shadows (L-shape)

            case 1:
            case 2:
            case 4:
            case 8: return 3; // 3 shadows (U-shape)

            case 0: return 4;// 4 shadows

            case 5:
            case 10: return 5; // 2 shadows (| | - shape, corridor)











            //case 0: return 4; // 4 shadows
            //case 1: return 3; // 3 shadows
            //case 2: return 3;
            //case 3: return 2; // 2 shadows (l-shape)
            //case 4: return 3;
            //case 5: return 5; // 2 shadows (| | - shape, corridor)
            //case 6: return 2;
            //case 7: return 1;
            //case 8: return 3;
            //case 9: return 2;
            //case 10: return 5;
            //case 11: return 1;
            //case 12: return 2;
            //case 13: return 1;
            //case 14: return 1;
            //case 15: return 0;


            default:
                Debug.LogWarning("Scripted tile tried to get non existing sprite");
                break;
        }
        return -1;

        //System.Random randGen = new System.Random();
        ////Random.seed = 123;
        ////Random.InitState(123);
        //int index = Random.Range(0, 100);
        //Debug.Log(index);
        //if (index <= 90)
        //{
        //    return 0;
        //}
        //else
        //{
        //    return Random.Range(1, 5);
        //}


    }

    private Quaternion GetRotation(byte mask)
    {
        switch (mask) //fix after getindex
        {
            case 0:
            case 1:
            case 3:
            case 5:
            case 13:
            case 15: break;

            case 2: return Quaternion.Euler(0f, 0f, -90f);
            case 6: return Quaternion.Euler(0f, 0f, -90f);
            case 11: return Quaternion.Euler(0f, 0f, -90f);

            case 4:
            case 7:
            case 12: return Quaternion.Euler(0f, 0f, 180f);

            case 8:
            case 9:
            case 10:
            case 14: return Quaternion.Euler(0f, 0f, 90f);

        }
        return Quaternion.Euler(0f, 0f, 0f);
    }

    private bool HasTile(ITilemap tilemap, Vector3Int position)
    {
        return tilemap.GetTile(position) == this;
    }

#if UNITY_EDITOR
    // The following is a helper that adds a menu item to create a RoadTile Asset
    [MenuItem("Assets/Create/ShadowTile")]
    public static void CreateGroundTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Shadow Tile", "New Shadow Tile", "Asset", "Save Shadow Tile", "Assets/Sprites/Tilesets");
        if (path == "")
            return;
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<ScriptedShadowTile>(), path);
    }
#endif
}
