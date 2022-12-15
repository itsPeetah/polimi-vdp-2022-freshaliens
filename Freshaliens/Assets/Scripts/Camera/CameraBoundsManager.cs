using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CameraBoundsManager : MonoBehaviour
{
    [SerializeField] private Vector3[] samplePoints = new Vector3[] {new Vector3(-200, -50), new Vector3(200, -50)};
    private float minXCoord = 0;
    private float maxXCoord = 0;

    [SerializeField] private Vector3 leftBound = new Vector3(-20, 0), rightBound = new Vector3(20, 0);
    public Vector3 LeftBound
    {
        get => leftBound;
        set
        {
            if (value.x > rightBound.x)
            {
                leftBound = rightBound;
                rightBound = value;
            }
            else leftBound = value;
        }
    }
    public Vector3 RightBound
    {
        get => rightBound;
        set
        {
            if (value.x < leftBound.x)
            {
                rightBound = leftBound;
                leftBound = value;
            }
            else rightBound = value;
        }
    }

    public Vector3[] SamplePoints
    {
        get => samplePoints;
        set
        {
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

    public Vector3 GetLowerBoundAtXCoord(float x)
    {
        if (samplePoints == null || samplePoints.Length == 0) return Vector3.down * float.MaxValue;
        if (x <= minXCoord) return samplePoints[0];
        if (x >= maxXCoord) return samplePoints[samplePoints.Length - 1];

        float n = 0;
        float d = 0;
        int j = 0;
        for (int i = 1; i < samplePoints.Length; i++)
        {
            j = i - 1;
            n = x - samplePoints[j].x;
            d = samplePoints[i].x - samplePoints[j].x;
            if (samplePoints[i].x >= x)
            {
                return Vector3.Lerp(samplePoints[j], samplePoints[i], n / d);
            }
        }
        return Vector3.down * float.MaxValue;
    }

    public Vector3 ClampPositionAlongXAxis(Vector3 position)
    {
        return new Vector3
        {
            x = Mathf.Clamp(position.x, leftBound.x, rightBound.x),
            y = position.y,
            z = position.z
        };
    }

    public void ClampOrthographicCamera(ref float x, ref float y, float cameraOrthoSize, float cameraAspectRatio) {
        float xSize = cameraOrthoSize * cameraAspectRatio;
        x = Mathf.Clamp(x, leftBound.x + xSize, rightBound.x - xSize);
        y = Mathf.Max(y, GetLowerBoundAtXCoord(x).y + cameraOrthoSize);
    }


}
