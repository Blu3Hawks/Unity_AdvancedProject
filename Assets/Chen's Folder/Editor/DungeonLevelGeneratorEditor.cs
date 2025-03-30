using System.Collections;
using System.Collections.Generic;
using Chen_s_Folder.Scripts.Procedural_Generation;
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
            dungeonLevelGenerator.ClearWorldInEditor();
            dungeonLevelGenerator.seed++; //we will jst add 1 to the seed, I am too tired for that sht
            dungeonLevelGenerator.GenerateLevel();
        }
    }
}
