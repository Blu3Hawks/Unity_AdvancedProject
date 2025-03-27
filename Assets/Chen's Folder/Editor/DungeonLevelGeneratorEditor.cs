using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DungeonLevelGenerator))]
public class DungeonLevelGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DungeonLevelGenerator dungeonLevelGenerator = (DungeonLevelGenerator)target;
        if (GUILayout.Button("Create New Level"))
        {
            dungeonLevelGenerator.GenerateLevel();
        }
    }
}
