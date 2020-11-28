using SF.Service;

namespace SF {
    public class StageContext {
        public StageContext(CharacterService characterService) {
            CharacterService = characterService;
        }

        public CharacterService CharacterService { get; }
    }
}
