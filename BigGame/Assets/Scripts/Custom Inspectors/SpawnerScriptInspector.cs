using System.Linq;
using UnityEditor;
using UnityEngine;
using static EnemySpawnClass;

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


        GUILayout.BeginHorizontal();

        int x = SpawnerScript.WhatEnemyCanSpawn.Count();
        for (int i = 0; i < x; i++)
        {
            if (buttonCount == buttonsInRow)
            {
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                buttonCount = 0;
            }            
            if (SpawnerScript.WhatEnemyCanSpawn[i].unitToSpawn!=null)
            {
                if (GUILayout.Button($"{SpawnerScript.WhatEnemyCanSpawn[i].unitToSpawn.name}"))
                {
                    SpawnerScript.SpawnEnemyUnit(i);
                }
            }
            else
            {
                GUILayout.Button("error/Nan");
            }


            buttonCount++;
        }

        GUILayout.EndHorizontal(); 

        EditorGUI.EndDisabledGroup();
    }
}




