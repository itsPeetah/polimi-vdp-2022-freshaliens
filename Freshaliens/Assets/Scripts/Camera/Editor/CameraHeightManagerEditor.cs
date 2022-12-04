using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CameraHeightManager))]
public class CameraHeightManagerEditor : Editor
{
    private CameraHeightManager chm;

    private void OnEnable()
    {
        chm = target as CameraHeightManager;
    }

    private void OnSceneGUI()
    {
        EditorGUI.BeginChangeCheck();
        Vector3[] positions = chm.SamplePoints;

        for (int i = 0; i < positions.Length; i++)
        {
            Vector3 p = Handles.PositionHandle(positions[i], Quaternion.identity);
            positions[i] = p;

            Handles.color = Color.yellow;
            if (i > 0) Handles.DrawLine(positions[i-1], positions[i]);
        }

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(chm, "Change Look At Target Position");
            chm.SamplePoints = positions;
        }


    }
}
