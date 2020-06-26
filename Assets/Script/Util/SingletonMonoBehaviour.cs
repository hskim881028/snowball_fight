using UnityEngine;
    
public class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T> {
    static T mInstance;

    public static T Instance {
        get {
            mInstance = FindObjectOfType<T>();
            if (mInstance == null) {
                GameObject go = new GameObject(typeof(T).Name);
                mInstance = go.AddComponent<T>();
            }

            return mInstance;
        }
    }
}