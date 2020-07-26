namespace hskim {
    public class StageContext {
        public CharacterService CharacterService { get; }
        
        public StageContext(CharacterService characterService) {
            CharacterService = characterService;
        }
    }
}