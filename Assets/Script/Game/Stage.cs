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
            var character = LoadDummyCharacter(1);
            Services = new Services(joystickController, character);
        }

        private void Update() {
            Services.Update();
        }

        private void LateUpdate() {
            Services.LateUpdate();
        }

        private Character.Character LoadDummyCharacter(int id) {
            return PrefabLoader.LoadCharacter(id, transform);
        }
    }
}
