using SF.Common.Util;
using SF.Service;
using UnityEngine;

namespace SF {
    public class Stage : SingletonMonoBehaviour<Stage> {
        private Transform _canvas;

        private Services Services { get; set; }

        private Client _client;

        private void Awake() {
            _canvas = GetComponentInChildren<Canvas>().transform;
            _client = new Client();
            
            PrefabLoader.LoadMenuUI(_canvas, _client);
            
            // var joystickController = PrefabLoader.LoadJoyStick(_canvas);
            // var character = LoadDummyCharacter(1);
            // Services = new Services(joystickController, character);
        }

        private void Update() {
            Services?.Update();
            _client?.Update();
        }

        private void LateUpdate() {
            Services?.LateUpdate();
        }

        private void OnDestroy() {
            _client?.Destroy();
        }

        private Character.Character LoadDummyCharacter(int id) {
            return PrefabLoader.LoadCharacter(id, transform);
        }
    }
}
