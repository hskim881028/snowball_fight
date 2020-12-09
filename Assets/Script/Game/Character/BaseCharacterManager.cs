using System.Collections;
using System.Collections.Generic;

namespace SF.Character {
    public abstract class BaseCharacterManager : IEnumerable<Character> {
        public abstract IEnumerator<Character> GetEnumerator();
        
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public abstract void UpdateLogic();
    }
}
