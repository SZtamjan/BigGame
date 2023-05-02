using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PatchControler))]
public class PathControllerInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PatchControler patchControler = (PatchControler)target;

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
