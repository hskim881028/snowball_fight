using System;
using UnityEngine;

namespace hskim {
    public class Stage : SingletonMonoBehaviour<Stage> {
        Transform mCanvas;
        
        Services Services { get; set; }

        void Awake() {
            mCanvas = GetComponentInChildren<Canvas>().transform;
            InputService inputService = PrefabLoader.LoadInputService(mCanvas);
            Joystick joystick = PrefabLoader.LoadJoyStick(mCanvas);
            Services = new Services(inputService, joystick);
            LoadDummyCharacter();
        }

        void Update() {
            Services.Update();
        }

        void LoadDummyCharacter() {
            const int id = 1;
            var character = PrefabLoader.LoadCharacter(id, transform);
            Services.CharacterService.Add(id, character);
        }
    }
}