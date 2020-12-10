﻿using System.Collections;
using System.Collections.Generic;

namespace SF.Character {
    public abstract class BaseCharacterManager : IEnumerable<Character> {
        public abstract int Count { get; }
        public abstract void UpdateLogic();
        public abstract IEnumerator<Character> GetEnumerator();
        
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
