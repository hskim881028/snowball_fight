namespace hskim {
    public class Services {
        readonly InputService mInputService;
        readonly CommandService mCommandService;
        readonly CharacterService mCharacterService;
        readonly StageContext mStageContext;
        
        public CharacterService CharacterService => mCharacterService;
        
        public Services(InputService inputService, JoystickController joystickController, Character character) {
            mCommandService = new CommandService();
            mCharacterService = new CharacterService();
            mCharacterService.Add(character.Data.Id, character);
            
            mInputService = inputService;
            mInputService.Init(mCommandService, joystickController);
            
            mStageContext = new StageContext(mCharacterService);
        }

        public void Update() {
            mCommandService.Update(mStageContext);
        }

        public void LateUpdate() {
            mCommandService.UpdateRunningCommands();
        }
    }
}