using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Freshaliens.Interaction.Components
{
    public class InvisiblePlatform : Interactable
    {
        //Parameters
        [Range(0.5f, 1f)]
        [SerializeField] private float _maxAlpha;
        [Range(0, 0.5f)]
        [SerializeField] private float _minAlpha;
        [SerializeField] private float _activeTimeAfterFairyExit = 1f;

        //State
        private GameObject _thisGameObject;
        private GameObject _childGameObject;
        private SpriteRenderer _spriteRenderer;


        void Start()
        {
            _thisGameObject = gameObject;
            _childGameObject = _thisGameObject.GetComponentInChildren<Transform>().GetChild(0).gameObject;
            _spriteRenderer = GetComponent<SpriteRenderer>();

            ChangeAlpha(_minAlpha);
            _childGameObject.SetActive(false);
        }

        private void ChangeAlpha(float newAlpha)
        {
            Color currentColor = _spriteRenderer.color;
            currentColor.a = newAlpha;
            _spriteRenderer.color = currentColor;
        }

        protected void ChangeLayer(string newLayerName)
        {
            int newLayer = LayerMask.NameToLayer(newLayerName);
            _thisGameObject.layer = newLayer;
        }


        public override void OnFairyEnter()
        {
            ChangeAlpha(_maxAlpha);
            _childGameObject.SetActive(true);
        }

        public override void OnFairyExit()
        {
            StartCoroutine(DeactivateAfterTimer());
        }

        IEnumerator DeactivateAfterTimer()
        {
            yield return new WaitForSeconds(_activeTimeAfterFairyExit);
            ChangeAlpha(_minAlpha);
            _childGameObject.SetActive(false);
            yield return null;
        }

    }
}