using SF.Common.Util;
using SF.Service;
using UnityEngine;

namespace SF {
    public class Stage : SingletonMonoBehaviour<Stage> {
        private Transform _canvas;
        private Services Services { get; set; }

        private void Awake() {
            _canvas = GetComponentInChildren<Canvas>().transform;
            var joystickController = PrefabLoader.LoadJoyStick(_canvas);
            Services = new Services(_canvas, transform, joystickController);
        }

        private void Update() {
            Services?.Update();
        }

        private void LateUpdate() {
            Services?.LateUpdate();
        }

        private void OnDestroy() {
            Services?.Destroy();
        }
    }
}