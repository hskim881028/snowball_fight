using System.Collections.Generic;
using UnityEngine;

namespace hskim.Command {
    [CommandMapping(ECommandType.Move)]
    public class MoveCommand : BaseCommand {
        public Vector2 mDelta;
        public int mID;
    }

    public class MoveCommandHandler : CommandHandler<MoveCommand> {
        protected override IEnumerator<CustomYieldInstruction> Execute(StageContext context, MoveCommand command) {
            var character = context.CharacterService.GetCharacter(command.mID);
            character.Controller.Move(command.mDelta);
            yield break;
        }
    }
}