using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CameraBoundsManager))]
public class CameraBoundsManagerEditor : Editor
{
    private CameraBoundsManager cbm;

    private void OnEnable()
    {
        cbm = target as CameraBoundsManager;
    }

    private void OnSceneGUI()
    {
        EditorGUI.BeginChangeCheck();
        Vector3[] positions = cbm.SamplePoints;

        for (int i = 0; i < positions.Length; i++)
        {
            Vector3 p = Handles.PositionHandle(positions[i], Quaternion.identity);
            positions[i] = p;

            Handles.color = Color.yellow;
            if (i > 0) Handles.DrawLine(positions[i-1], positions[i], 3f);
            Handles.Label(positions[i] + Vector3.down, $"SAMPLE POINT #{i}", EditorStyles.boldLabel);
        }

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(cbm, "Changed min-y bound in camera manager");
            EditorUtility.SetDirty(cbm);
            cbm.SamplePoints = positions;
        }

        EditorGUI.BeginChangeCheck();
        Vector3 leftBound = Handles.PositionHandle(cbm.LeftBound, Quaternion.identity);
        Handles.color = Color.blue;
        Handles.DrawLine(leftBound + Vector3.down * 80, leftBound + Vector3.up * 80, 3f);
        Handles.color = Color.white;
        Handles.Label(leftBound, "CAMERA LEFT BOUND", EditorStyles.boldLabel);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(cbm, "Changed camera left bound");
            EditorUtility.SetDirty(cbm);
            cbm.LeftBound= leftBound;
        }
        

        EditorGUI.BeginChangeCheck();
        Vector3 rightBound = Handles.PositionHandle(cbm.RightBound, Quaternion.identity);
        Handles.color = Color.blue;
        Handles.DrawLine(rightBound + Vector3.down * 80, rightBound + Vector3.up * 80, 3f);
        Handles.color = Color.white;
        Handles.Label(rightBound, "CAMERA RIGHT BOUND",EditorStyles.boldLabel);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(cbm, "Changed camera left bound");
            EditorUtility.SetDirty(cbm);
            cbm.RightBound = rightBound;
        }

        

    }
}
