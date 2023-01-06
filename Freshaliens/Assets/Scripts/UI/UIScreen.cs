using System;
using UnityEngine;

namespace Freshaliens.UI
{
    public class UIScreen : MonoBehaviour
    {
        public static event Action onButtonClick;
        [SerializeField] protected GameObject root;

        protected virtual void Start()
        {
            if (!root) root = transform.GetChild(0).gameObject;
        }

        public void PlayButtonSound()
        {
            onButtonClick?.Invoke();
        }

        public virtual void SetActive(bool active)
        {
            if(root!=null) root.SetActive(active);
        }
    }
}