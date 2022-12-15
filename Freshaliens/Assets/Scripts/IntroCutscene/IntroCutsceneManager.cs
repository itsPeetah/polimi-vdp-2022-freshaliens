using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Freshaliens.CutScene
{
    public class IntroCutsceneManager : MonoBehaviour
    {
        [SerializeField] private CutsceneSlide[] slides = new CutsceneSlide[] { };

        [Header("UI")]
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Image image;

        private int currentSlide = 0;

        private void Start()
        {
            currentSlide = 0;
            if (slides.Length > 0) SetSlide(currentSlide);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) {
                currentSlide++;
                if (currentSlide >= slides.Length)
                {
                    SceneLoadingManager.LoadLevelSelection();
                }
                else SetSlide(currentSlide);
            }
        }

        private void SetSlide(int idx) {
            text.SetText(slides[idx].Text);
            image.sprite = slides[idx].Sprite;
        }
    }
}