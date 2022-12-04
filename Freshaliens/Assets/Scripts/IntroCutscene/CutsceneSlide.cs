using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Freshaliens.CutScene
{
    [CreateAssetMenu(menuName = "Freshaliens/Cutscene Slide", fileName = "New Slide")]
    public class CutsceneSlide : ScriptableObject
    {
        [SerializeField, TextArea] private string text = "";
        public string Text => text;

        [SerializeField] private Sprite sprite = null;
        public Sprite Sprite => sprite;
    }
}