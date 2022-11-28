using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Freshaliens.LevelSelection.Components
{
    public class LevelSelectionManager : MonoBehaviour
    {
        private static LevelSelectionManager instance;
        public static LevelSelectionManager Instance => instance;

        [SerializeField] private LevelRep[] levels;
        private int lastSelectedLevel = 0;
        private int currentlySelectedLevel = 0;
        [SerializeField] private bool allowLockedSelection = false;

        [Header("Visuals")]
        [SerializeField] private GameObject levelMapLinePrefab = null;
        private Transform levelMapLineContainer;
        [Space(10)]
        [SerializeField] private LevelSelectionCursor cursor;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            currentlySelectedLevel = 0; // TODO Load latest from saved data;

            InstantiateMapLines();
            cursor.SetPosition(levels[currentlySelectedLevel].transform.position);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.RightArrow)) SelectNextLevel();
            if (Input.GetKeyDown(KeyCode.LeftArrow)) SelectPrevLevel();
        }

        private void SelectLevel(int index)
        {
            if (index > PlayerData.Instance.LastUnlockedLevel && !allowLockedSelection) {
                Debug.Log($"Level has not been unlocked yet!");
                return;
            }


            int prevSelectedLevel = currentlySelectedLevel;
            currentlySelectedLevel = Mathf.Abs(index) % levels.Length;

            int diff = currentlySelectedLevel - prevSelectedLevel;
            if (diff > 1)
            {
                for (int i = 1; i < levels.Length; i++)
                    cursor.SetTarget(i, levels[i].transform.position);
            }
            else if (diff < -1) {
                for (int i = levels.Length - 2; i >= 0; i--)
                    cursor.SetTarget(i, levels[i].transform.position);
            }
            else
                cursor.SetTarget(currentlySelectedLevel, levels[currentlySelectedLevel].transform.position);
        }
        private void SelectNextLevel() => SelectLevel(currentlySelectedLevel + 1);
        private void SelectPrevLevel() => SelectLevel(currentlySelectedLevel - 1 + levels.Length);

        private void InstantiateMapLines() {

            levelMapLineContainer = new GameObject("Map Lines").transform;
            levelMapLineContainer.position = Vector3.zero;

            float maxDistance = Vector3.Distance(levels[0].transform.position, levels[levels.Length - 1].transform.position);

            for (int i = 1; i < levels.Length; i++) {
                LevelMapLine lml = Instantiate(levelMapLinePrefab, levelMapLineContainer).GetComponent<LevelMapLine>();
                lml.SetPoints(levels[i - 1].transform, levels[i].transform, maxDistance);
            }
        }
    }
}