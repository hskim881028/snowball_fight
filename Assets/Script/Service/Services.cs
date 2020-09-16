namespace hskim {
    public class Services {
        private readonly CommandService _commandService;
        private readonly InputService _inputService;
        private readonly StageContext _stageContext;

        public Services(InputService inputService, JoystickController joystickController, Character character) {
            _commandService = new CommandService();
            CharacterService = new CharacterService();
            CharacterService.Add(character.Data.Id, character);

            _inputService = inputService;
            _inputService.Init(_commandService, joystickController);

            _stageContext = new StageContext(CharacterService);
        }

        public CharacterService CharacterService { get; }

        public void Update() {
            _commandService.Update(_stageContext);
        }

        public void LateUpdate() {
            _commandService.UpdateRunningCommands();
        }
    }
}