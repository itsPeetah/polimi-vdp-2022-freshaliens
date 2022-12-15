using UnityEngine;

namespace Freshaliens.UI
{
    public class LevelUIScreen : MonoBehaviour
    {
        [SerializeField] protected GameObject root;

        protected virtual void Start()
        {
            if (!root) root = transform.GetChild(0).gameObject;
        }

        public virtual void SetActive(bool active)
        {
            if(root!=null) root.SetActive(active);
        }
    }
}