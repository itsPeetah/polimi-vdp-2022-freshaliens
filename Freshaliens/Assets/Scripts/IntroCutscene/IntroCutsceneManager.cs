using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Freshaliens.CutScene
{
    public class IntroCutsceneManager : MonoBehaviour
    {
        [SerializeField] private float scrollSpeed = 100f;
        [SerializeField] private float fastSpeedMultiplier = 4f;
        [SerializeField] private float yThreshold = 1080f;
        [SerializeField] private Transform scrollTransform = null;
        [SerializeField] private AudioSource audioSource = null;
        private bool done = false;
        private bool speedUp = false;

        private void Start()
        {
            PlayerData data = PlayerData.Instance;

            if (data.MuteMaster || data.MuteMusic) audioSource.volume = 0;
            else audioSource.volume *= data.MusicVolume * data.MasterVolume;
        }

        private void Update()
        {
            if (done) return;

            if (Input.GetKeyDown(KeyCode.Space)) speedUp = !speedUp;

            float speedMultiplier = speedUp ? fastSpeedMultiplier : 1f;
            float speed = scrollSpeed * speedMultiplier * Time.deltaTime;
            float y = scrollTransform.position.y;
            
            if (y > yThreshold)
            {
                done = true;
                SceneLoadingManager.LoadLevelSelection();
            }
            else scrollTransform.Translate(Vector3.up * speed);
        }
    }
}