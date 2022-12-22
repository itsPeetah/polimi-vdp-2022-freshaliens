using System.Collections;
using UnityEngine;
using Freshaliens.Enemy.Components;
using UnityEditor;

namespace Freshaliens.Enemy.Editor
{
    [CustomEditor(typeof(EnemyPatrol))]
    public class EnemyPatrolEditor : UnityEditor.Editor
    {
        private EnemyPatrol enemy;

        private void OnEnable()
        {
            enemy = target as EnemyPatrol;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
        }

        private void OnSceneGUI()
        {
            EditorGUI.BeginChangeCheck();
            Vector3 startingPosition = Handles.PositionHandle(enemy.StartPosition, Quaternion.identity);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(enemy, "Change Look At Target Position");
                enemy.StartPosition = startingPosition;
            }
            Handles.Label(startingPosition, "PATROL START POSITION", EditorStyles.boldLabel);

            EditorGUI.BeginChangeCheck();
            Vector3 endPosition = Handles.PositionHandle(enemy.EndPosition, Quaternion.identity);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(enemy, "Change Look At Target Position");
                enemy.EndPosition = endPosition;
            }
            Handles.Label(endPosition, "PATROL END POSITION", EditorStyles.boldLabel);
        }
    }
}