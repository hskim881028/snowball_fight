namespace hskim {
    public readonly struct Character {
        public CharacterData Data { get; }
        public CharacterController Controller { get; }
        public CharacterView View { get; }

        public Character(CharacterData data, CharacterController controller, CharacterView view) {
            Data = data;
            Controller = controller;
            View = view;
        }
    }
}