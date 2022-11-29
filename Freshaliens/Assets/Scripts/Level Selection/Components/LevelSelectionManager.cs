using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Freshaliens.LevelSelection.Components
{
    public class LevelSelectionManager : MonoBehaviour
    {
        private static LevelSelectionManager instance;
        public static LevelSelectionManager Instance => instance;

        [Header("Level data")]
        [SerializeField] private LevelRep[] levels;
        [SerializeField] private bool allowLockedSelection = false;

        [Header("Visuals")]
        [SerializeField] private GameObject levelMapLinePrefab = null;
        [SerializeField] private LevelSelectionCursor cursor = null;

        [Header("UI")]
        [SerializeField] private LevelSelectionUI ui = null;

        // State
        private LevelInfo currentLevelInfo = null;
        private int currentlySelectedLevel = 0;
        private Transform levelMapLineContainer;
        private LevelMapLine[] mapLines = null;

        // Events
        public event Action<LevelInfo> onLevelSelected = null;

        // Properties
        public LevelInfo CurrentLevelInfo => currentLevelInfo;

        private void Awake()
        {
            instance = this;
            currentLevelInfo = levels[0].Info;
        }

        private void Start()
        {
            currentlySelectedLevel = PlayerData.Instance.LastUnlockedLevel;

            InstantiateMapLines();
            UpdateMapLineState();

            // Subscribe events
            onLevelSelected += ui.UpdateLevelInfoDisplay;

            SelectLevel(currentlySelectedLevel);
            
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) SelectNextLevel();
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) SelectPrevLevel();

#if UNITY_EDITOR

            if (Input.GetKeyDown(KeyCode.P))
            {
                PlayerData.Instance.LastUnlockedLevel = PlayerData.Instance.LastUnlockedLevel + 1;

                UpdateMapLineState();
            }
#endif
        }

        private void SelectLevel(int index)
        {

            int prevSelectedLevel = currentlySelectedLevel;
            int levelToSelect = Mathf.Abs(index) % levels.Length;

            if (levelToSelect > PlayerData.Instance.LastUnlockedLevel && !allowLockedSelection)
            {
                //Debug.Log($"Level has not been unlocked yet!");
                return;
            }

            currentlySelectedLevel = levelToSelect;
            currentLevelInfo = levels[currentlySelectedLevel].Info;

            int diff = currentlySelectedLevel - prevSelectedLevel;
            if (diff == 0)
            {
                cursor.SetPosition(levels[currentlySelectedLevel].transform.position);
            }
            else if (diff > 1)
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

            onLevelSelected?.Invoke(currentLevelInfo);
        }
        private void SelectNextLevel() => SelectLevel(currentlySelectedLevel + 1);
        private void SelectPrevLevel() => SelectLevel(currentlySelectedLevel - 1 + levels.Length);

        private void InstantiateMapLines() {

            levelMapLineContainer = new GameObject("Map Lines").transform;
            levelMapLineContainer.position = Vector3.zero;

            float maxDistance = Vector3.Distance(levels[0].transform.position, levels[levels.Length - 1].transform.position);

            mapLines = new LevelMapLine[levels.Length - 1];
            for (int i = 1; i < levels.Length; i++) {
                LevelMapLine lml = Instantiate(levelMapLinePrefab, levelMapLineContainer).GetComponent<LevelMapLine>();
                lml.SetPoints(levels[i - 1].transform, levels[i].transform, maxDistance);
                mapLines[i - 1] = lml;
            }
        }

        private void UpdateMapLineState() {
            for (int i = 0; i < mapLines.Length; i++) {
                mapLines[i].SetUnlocked(i+1 <= PlayerData.Instance.LastUnlockedLevel);
            }
        }
    }
}