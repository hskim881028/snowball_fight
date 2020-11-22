using System.Collections.Generic;
using UnityEngine;

namespace hskim.Action {
    [ActionMapping(EActionType.Move)]
    public class MoveAction : BaseAction {
        public Vector2 mDelta;
        public int mID;
    }

    public class MoveActionHandler : ActionHandler<MoveAction> {
        protected override IEnumerator<CustomYieldInstruction> Execute(StageContext context, MoveAction action) {
            var character = context.CharacterService.GetCharacter(action.mID);
            character.Controller.Move(action.mDelta);
            yield break;
        }
    }
}