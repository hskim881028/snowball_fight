using UnityEngine;

namespace hskim {
    public static class PrefabLoader {
        public static Character LoadCharacter(int id, Transform parent) {
            var prefab = Resources.Load($"Prefab/Character/Character_{id:D3}") as GameObject;
            GameObject clone = Object.Instantiate(prefab, parent);
            CharacterView view = clone.GetComponent<CharacterView>();
            CharacterData data = new CharacterData(id);
            CharacterVariableData variableData = new CharacterVariableData(Vector2.zero, 10);
            return new Character(data, new CharacterController(data, variableData, view), view);
        }
        
        public static JoystickController LoadJoyStick(Transform parent) {
            var prefab = Resources.Load("Prefab/Joystick/Joystick") as GameObject;
            GameObject clone = Object.Instantiate(prefab, parent);
            return new JoystickController(new JoystickData(), clone.GetComponent<JoystickView>());
        }
        
        public static InputService LoadInputService(Transform parent) {
            var prefab = Resources.Load("Prefab/Input/Input") as GameObject;
            return Object.Instantiate(prefab, parent).GetComponent<InputService>();
        }
    }
}