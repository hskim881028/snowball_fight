using System.Collections.Generic;
using UnityEngine;

namespace SF.Action {
    [ActionMapping(EActionType.Move)]
    public class MoveAction : BaseAction {
        public Vector2 Position;
        public int Id;
    }

    public class MoveActionHandler : ActionHandler<MoveAction> {
        protected override IEnumerator<CustomYieldInstruction> Execute(StageContext context, MoveAction action) {
            var character = context.CharacterService.GetCharacter(action.Id);
            character.Controller.Move(action.Position);
            yield break;
        }
    }
}