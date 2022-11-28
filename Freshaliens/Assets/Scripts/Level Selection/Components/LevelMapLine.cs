using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Freshaliens.LevelSelection.Components
{
    public class LevelMapLine : MonoBehaviour
    {
        private const string MAT_SEGMENTATION = "_Segmentation";

        [SerializeField] private LineRenderer line;

        [SerializeField] private float materialSegmentLength = 5f;
        //[SerializeField] private float 

        private Transform start = null, end = null;
        private float distanceRatio = 0;

        public void SetPoints(Transform start, Transform end, float maxDistance) {
            this.start = start;
            this.end = end;
            line.SetPositions(new Vector3[] {start.position, end.position});

            float length = Vector3.Distance(start.position, end.position);
            distanceRatio = length / maxDistance;
            line.material.SetFloat(MAT_SEGMENTATION, CalculateMaterialSegmentation());
        }

#if UNITY_EDITOR
        private void Update()
        {
            line.material.SetFloat(MAT_SEGMENTATION, CalculateMaterialSegmentation());
        }
#endif
        private float CalculateMaterialSegmentation() {

            float segmentOnFullLengthRatio = distanceRatio * materialSegmentLength;
            return segmentOnFullLengthRatio;
        }
    }
}