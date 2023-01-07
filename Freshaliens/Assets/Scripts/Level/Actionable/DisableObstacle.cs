using System;
using UnityEngine;

namespace Freshaliens.Interaction.Components
{
    public  class DisableObstacle : Actionable
    {
        private bool _isActive;

        private void Start()
        {
            _isActive = true;
        }
        
        private void ToggleState()
        {
            _isActive = !_isActive;
            gameObject.SetActive(_isActive);
        }

        public override void OnAction()
        {
            ToggleState();
        }
    }
}