using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Freshaliens.LevelSelection.Components
{
    public class LevelMapLine : MonoBehaviour
    {
        private const string MAT_SEGMENTATION = "_Segmentation";

        [SerializeField] private LineRenderer line;

        [Header("Visuals")]
        [SerializeField] private float materialSegmentLength = 5f;
        [SerializeField] private Color lockedColor = Color.gray, unlockedColor = Color.yellow;

        private float distanceRatio = 0;

#if UNITY_EDITOR
        private void Update()
        {
            line.material.SetFloat(MAT_SEGMENTATION, CalculateMaterialSegmentation());
        }
#endif

        public void SetPoints(Transform start, Transform end, float maxDistance) {
            line.SetPositions(new Vector3[] {start.position, end.position});

            float length = Vector3.Distance(start.position, end.position);
            distanceRatio = length / maxDistance;
            line.material.SetFloat(MAT_SEGMENTATION, CalculateMaterialSegmentation());
        }

        public void SetUnlocked(bool value) {
            line.startColor = value ? unlockedColor : lockedColor;
            line.endColor = value ? unlockedColor : lockedColor;
        }

        private float CalculateMaterialSegmentation() {

            float segmentOnFullLengthRatio = distanceRatio * materialSegmentLength;
            return segmentOnFullLengthRatio;
        }
    }
}