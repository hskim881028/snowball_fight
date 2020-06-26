using System.Collections;
using UnityEngine;

namespace hskim.Command {
    [CommandMapping(ECommandType.Move)]
    public class MoveCommand : BaseCommand {
        public int id;
        public Vector2 delta;
    }

    public class MoveCommandHandler : CommandHandler<MoveCommand> {
        protected override IEnumerator Execute(StageContext context, MoveCommand command) {
            Character character = context.CharacterService.GetCharacter(command.id);
            character.Controller.Move(command.delta);
            yield break;
        }
    }
}