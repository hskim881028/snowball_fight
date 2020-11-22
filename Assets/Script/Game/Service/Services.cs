using UnityEngine;

namespace hskim {
    public class Services {
        private readonly CommandService _commandService;
        private readonly InputService _inputService;

        public Services(JoystickController joystickController, Character character) {
            var characterService = new CharacterService();
            characterService.Add(character.Data.Id, character);
            var stageContext = new StageContext(characterService);
            _commandService = new CommandService(stageContext);
            _inputService = new InputService(_commandService, joystickController);
        }

        public void Update() {
            _inputService.Update();
            _commandService.Update();
        }

        public void LateUpdate() {
            _commandService.LateUpdate();
        }
    }
}
