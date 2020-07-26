using UnityEngine;

namespace hskim {
    public class CharacterController {
        CharacterData mData;
        CharacterVariableData mVariableData;
        readonly CharacterView mView;

        public CharacterController(CharacterData data, 
                                   CharacterVariableData variableData,
                                   CharacterView view) {
            mData = data;
            mVariableData = variableData;
            mView = view;
        }

        public void Move(Vector2 delta) {
            mVariableData.AddPosition(delta);
            mView.SetPosition(mVariableData.Position);
        }
    }
}