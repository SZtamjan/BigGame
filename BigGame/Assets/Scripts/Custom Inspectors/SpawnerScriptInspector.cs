using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SpawnerScript))]
public class SpawnerScriptInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SpawnerScript SpawnerScript = (SpawnerScript)target;

        bool isPlaying = EditorApplication.isPlaying;

        EditorGUI.BeginDisabledGroup(!isPlaying);

        int buttonsInRow = 3;
        int buttonCount = 0;


        if (GUILayout.Button($"Spawn Random"))
        {
            SpawnerScript.SpawnEnemyUnit();
        }
        GUILayout.BeginHorizontal(); 

        for (int i = 0; i < SpawnerScript.WhatEnemyCanSpawn.Count(); i++)
        {
            if (buttonCount == buttonsInRow)
            {
                GUILayout.EndHorizontal(); 
                GUILayout.BeginHorizontal(); 
                buttonCount = 0;
            }

            if (GUILayout.Button($"{SpawnerScript.WhatEnemyCanSpawn[i].name}"))
            {
                SpawnerScript.SpawnEnemyUnit(i);
            }

            buttonCount++;
        }

        GUILayout.EndHorizontal(); 

        EditorGUI.EndDisabledGroup();
    }
}




