namespace SF.Character {
    public readonly struct CharacterData {
        public int Id { get; }

        // public int Hp { get; }
        public string Name { get; }

        public CharacterData(int id, string name) {
            Id = id;
            // Hp = hp;
            Name = name;
        }
    }
}