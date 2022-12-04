using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CameraHeightManager : MonoBehaviour
{
    [SerializeField] private Vector3[] samplePoints = new Vector3[] { };
    private float minXCoord = 0;
    private float maxXCoord = 0;

    public Vector3[] SamplePoints {
        get => samplePoints;
        set {
            // Null check
            if (value == null) value = new Vector3[0];
            // Sort by X
            List<Vector3> sorted = value.ToList();
            sorted.Sort((a, b) => a.x.CompareTo(b.x));
            samplePoints = sorted.ToArray();
            // Get bounds
            if (samplePoints.Length == 0) return;
            minXCoord = samplePoints[0].x;
            maxXCoord = samplePoints[samplePoints.Length - 1].x;
        }
    }

    private void OnValidate()
    {
        // auto sort
        SamplePoints = samplePoints;
    }

    public Vector3 GetLowerBoundAtXCoord(float x) {
        if (samplePoints == null || samplePoints.Length == 0) return Vector3.down * float.MaxValue;
        if (x <= minXCoord) return samplePoints[0];
        if (x >= maxXCoord) return samplePoints[samplePoints.Length - 1];

        float n = 0;
        float d = 0;
        int j = 0;
        for (int i = 1; i < samplePoints.Length; i++) {
            j = i - 1;
            n = x - samplePoints[j].x;
            d = samplePoints[i].x - samplePoints[j].x;
            if (samplePoints[i].x >= x) {
                return Vector3.Lerp(samplePoints[j], samplePoints[i], n / d);
            }
        }
        return Vector3.down * float.MaxValue;

    }


}
