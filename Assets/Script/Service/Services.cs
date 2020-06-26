namespace hskim {
    public class Services {
        readonly InputService mInputService;
        readonly CommandService mCommandService;
        readonly CharacterService mCharacterService;
        readonly StageContext mStageContext;
        
        public CharacterService CharacterService => mCharacterService;
        
        public Services(InputService inputService, Joystick joystick) {
            mCommandService = new CommandService();
            mCharacterService = new CharacterService();
            
            mInputService = inputService;
            mInputService.Init(mCommandService, joystick);
            
            mStageContext = new StageContext(mCommandService, mCharacterService);
        }

        public void Update() {
            mCommandService.Update(mStageContext);
        }
    }
}