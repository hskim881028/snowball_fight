namespace hskim {
    public class StageContext {
        public CommandService CommandService { get; }
        public CharacterService CharacterService { get; }
        
        public StageContext(CommandService commandService, 
                            CharacterService characterService) {
            CommandService = commandService;
            CharacterService = characterService;
        }
    }
}