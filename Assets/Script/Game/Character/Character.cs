namespace SF.Character {
    public class Character {
        public Character(CharacterData data, CharacterController controller, CharacterView view) {
            Data = data;
            Controller = controller;
            View = view;
        }

        public CharacterData Data { get; }
        public CharacterController Controller { get; }
        public CharacterView View { get; }
    }
}