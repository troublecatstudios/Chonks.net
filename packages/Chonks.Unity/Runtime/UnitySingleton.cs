using System;
using UnityEngine;

namespace Chonks.Unity {
    /// <summary>
    /// Inherit from this base class to create a singleton.
    /// e.g. public class MyClassName : NoirSingletonBehaviour<MyClassName> {}
    /// </summary>
    public class UnitySingleton<T> : MonoBehaviour where T : MonoBehaviour {
        // Check to see if we're about to be destroyed.
        private static bool _shuttingDown = false;
        private static object _lock = new object();
        private static T _instance;

        private bool _started = false;

        public static bool IsAssigned() => _instance != null;

        /// <summary>
        /// Access singleton instance through this propriety.
        /// </summary>
        public static T Instance {
            get {
                if (_shuttingDown) {
                    Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
                        "' already destroyed. Returning null.");
                    return null;
                }

                if (_instance == null) {
                    lock (_lock) {
                        // Search for existing instance.
                        _instance = (T)FindObjectOfType(typeof(T), includeInactive: true);

                        // Create new instance if one doesn't already exist.
                        if (_instance == null) {
                            // Need to create a new GameObject to attach the singleton to.
                            var singletonObject = new GameObject();
                            _instance = singletonObject.AddComponent<T>();
                            singletonObject.name = typeof(T).ToString() + " (Singleton)";
                            DontDestroyOnLoad(singletonObject);
                        }
                    }
                }
                return _instance;
            }
        }

        private void Awake() {
            CheckInstance(SingletonAwake);
        }

        protected virtual void SingletonAwake() {

        }

        private void Start() {
            // Make instance persistent.
            _started = true;
            CheckInstance(SingletonStart);
        }

        protected virtual void SingletonStart() {

        }

        private void OnApplicationQuit() {
            _shuttingDown = true;
        }

        private void OnDestroy() {
            // only destroy if we actually received a start message
            _shuttingDown = _started && true;
        }

        private void CheckInstance(Action callback) {
            if (_instance && _instance != this) {
                Destroy(this);
                return;
            }
            callback();
        }
    }
}
