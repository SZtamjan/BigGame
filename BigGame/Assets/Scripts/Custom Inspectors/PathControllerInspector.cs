using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PathControler))]
public class PathControllerInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PathControler patchControler = (PathControler)target;

        bool isPlaying = EditorApplication.isPlaying;

        EditorGUI.BeginDisabledGroup(!isPlaying);

        if (GUILayout.Button("PlauerTurn"))
        {
            patchControler.PlayerUnitPhase();
        }
        if (GUILayout.Button("EnemyTurn"))
        {
            patchControler.ComputerUnitPhaze();
        }





        EditorGUI.EndDisabledGroup();
    }
}
