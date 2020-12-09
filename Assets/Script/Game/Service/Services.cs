using SF.Joystick;

namespace SF.Service {
    public class Services {
        private readonly ActionService _actionService;
        private readonly InputService _inputService;

        public Services(JoystickController joystickController, Character.Character character) {
            var characterService = new CharacterService();
            characterService.Add(character.Data.Id, character);
            var stageContext = new StageContext(characterService);
            _actionService = new ActionService(stageContext);
            _inputService = new InputService(_actionService, joystickController);
        }

        public void Update() {
            _inputService.Update();
            _actionService.Update();
        }

        public void LateUpdate() {
            _actionService.LateUpdate();
        }
    }
}
