using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapGenerator))]
public class MapGenerationFunctionCallButton : Editor
{
    public override void OnInspectorGUI()
    {
        MapGenerator mapGenerator = (MapGenerator)target;
        if (DrawDefaultInspector() && mapGenerator.autoUpdateMap)
        {
            mapGenerator.GenerateMap();
        }

        if(GUILayout.Button("Generate Map/Cave")){
            mapGenerator.GenerateMap();
        }
    }
}
