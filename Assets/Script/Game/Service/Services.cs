using SF.Joystick;
using UnityEngine;

namespace SF.Service {
    public class Services {
        private readonly ActionService _actionService;
        private readonly ClientService _clientService;
        private readonly InputService _inputService;
        private readonly ServerSerivce _serverSerivce;

        public Services(Transform canvas,
                        Transform stage,
                        JoystickController joystickController /*, Character.Character character*/) {
            var characterService = new CharacterService();
            var stageContext = new StageContext(characterService);
            _actionService = new ActionService(stageContext);
            _clientService = new ClientService(_actionService, characterService, stage);
            _serverSerivce = new ServerSerivce();
            _inputService = new InputService(_clientService, joystickController);

            PrefabLoader.LoadMenuUI(canvas, _serverSerivce, _clientService);
        }

        public void Update() {
            _inputService.Update(); // for local 
            _actionService.Update();
            _clientService?.Update();
            _serverSerivce?.Update();
        }

        public void LateUpdate() {
            _actionService.LateUpdate();
        }

        public void Destroy() {
            _clientService?.Destroy();
            _serverSerivce?.Destroy();
        }
    }
}