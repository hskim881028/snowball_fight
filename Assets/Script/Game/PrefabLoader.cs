using SF.Character;
using SF.Common.Util;
using SF.Joystick;
using SF.Service;
using SF.UI;
using UnityEngine;
using CharacterController = SF.Character.CharacterController;

namespace SF {
    public static class PrefabLoader {
        public static Character.Character LoadCharacter(int id, Transform parent) {
            var prefab = Resources.Load($"Prefab/Character/Character_{id:D3}") as GameObject;
            var clone = Object.Instantiate(prefab, parent);
            var view = clone.GetComponent<CharacterView>();
            clone.transform.position = Gameconfig.StartingPoint[id];
            var data = new CharacterData(id, string.Empty);
            var variableData = new CharacterVariableData(Gameconfig.StartingPoint[id]);
            return new Character.Character(data, new CharacterController(data, variableData, view), view);
        }

        public static JoystickController LoadJoyStick(Transform parent) {
            var prefab = Resources.Load("Prefab/Joystick/Joystick") as GameObject;
            var clone = Object.Instantiate(prefab, parent);
            return new JoystickController(new JoystickData(), clone.GetComponent<JoystickView>());
        }

        public static void LoadMenuUI(Transform parent, ServerSerivce server, ClientService clientService) {
            var prefab = Resources.Load("Prefab/UI/Menu") as GameObject;
            var clone = Object.Instantiate(prefab, parent);
            clone.GetComponent<MenuUI>().Init(server, clientService);
        }
    }
}