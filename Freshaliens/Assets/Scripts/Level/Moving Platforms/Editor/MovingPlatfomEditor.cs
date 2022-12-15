using UnityEngine;
using UnityEditor;

using Freshaliens.Level.Components;
namespace Freshaliens.Level.Editor
{
    [CustomEditor(typeof(MovingPlatform))]
    public class MovingPlatfomEditor : UnityEditor.Editor
    {
        private MovingPlatform platform;

        private void OnEnable()
        {
            platform = target as MovingPlatform;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
        }

        private void OnSceneGUI()
        {
            EditorGUI.BeginChangeCheck();
            Vector3 startingPosition = Handles.PositionHandle(platform.StartPosition, Quaternion.identity);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(platform, "Change Look At Target Position");
                platform.StartPosition = startingPosition;
            }

            EditorGUI.BeginChangeCheck();
            Vector3 endPosition = Handles.PositionHandle(platform.EndPosition, Quaternion.identity);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(platform, "Change Look At Target Position");
                platform.EndPosition = endPosition;
            }
        }
    }

}