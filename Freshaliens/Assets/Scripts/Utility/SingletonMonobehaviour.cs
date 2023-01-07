using System;
using UnityEngine;

namespace Freshaliens
{
    public abstract class SingletonMonobehaviour<T> : MonoBehaviour where T : SingletonMonobehaviour<T>
    {
        public class SingletonException : Exception {
            public SingletonException(string message) : base(message) { }
            public SingletonException(string message, Exception innerException) : base(message, innerException) { }
        }

        private static T instance = null;
        private static bool exists = false;

        public static bool Exists { get => exists; private set => exists = value; }

        public static T Instance
        {
            get
            {
                if (!Exists) throw new SingletonException($"Singleton instance for class {nameof(T)} is not assigned!");
                return instance;
            }
            private set
            {
                instance = value;
                Exists = value != null;
            }
        }

        [Header("Singleton Settings")]
        [SerializeField] protected bool persistSceneLoad = false;

        protected virtual void Awake() {

            if (exists)
            {
                Destroy(this.gameObject);
            }
            else {
                Instance = this as T;
                if (persistSceneLoad) {
                    DontDestroyOnLoad(this.gameObject);
                }
            }
        }

        protected virtual void OnDestroy() {
            if (Instance == this) Instance = null;
        }
    }
}
