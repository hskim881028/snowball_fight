﻿using UnityEngine;

namespace SF.Character {
    public class CharacterController {
        private readonly CharacterData _data;
        private readonly CharacterVariableData _variableData;
        private readonly CharacterView _view;

        public CharacterController(CharacterData data, CharacterVariableData variableData, CharacterView view) {
            _data = data;
            _variableData = variableData;
            _view = view;
        }

        public void Move(Vector2 position) {
            _variableData.Position = position;
            _view.SetPosition(_variableData.Position);
        }
    }
}