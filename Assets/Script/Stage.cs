using System;
using UnityEngine;

namespace hskim {
    public class Stage : SingletonMonoBehaviour<Stage> {
        Transform mCanvas;
        
        Services Services { get; set; }

        void Awake() {
            mCanvas = GetComponentInChildren<Canvas>().transform;
            InputService inputService = PrefabLoader.LoadInputService(mCanvas);
            JoystickController joystickController = PrefabLoader.LoadJoyStick(mCanvas);
            Character character = LoadDummyCharacter(1);
            Services = new Services(inputService, joystickController, character);
        }

        void Update() {
            Services.Update();
        }

        Character LoadDummyCharacter(int id) {
            return PrefabLoader.LoadCharacter(id, transform);
        }
    }
}