using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Freshaliens.LevelSelection.Components
{
    public class LevelMapLine : MonoBehaviour
    {
        [SerializeField] private LineRenderer line;
        private Transform start, end;

        public void SetPoints(Transform start, Transform end) {
            this.start = start;
            this.end = end;
            line.SetPositions(new Vector3[] {start.position, end.position});
        }
    }
}