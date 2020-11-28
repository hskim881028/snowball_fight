using UnityEngine;

namespace SF.Common.Util {
    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T> {
        private static T _instance;

        public static T Instance {
            get {
                _instance = FindObjectOfType<T>();
                if (_instance == null) {
                    var go = new GameObject(typeof(T).Name);
                    _instance = go.AddComponent<T>();
                }

                return _instance;
            }
        }
    }
}
