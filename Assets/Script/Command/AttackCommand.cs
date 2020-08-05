using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hskim.Command {
    [CommandMapping(ECommandType.Attack)]
    public class AttackBaseCommand : BaseCommand {
    }

    public class AttackCommandHandler : CommandHandler<AttackBaseCommand> {
        protected override IEnumerator<CustomYieldInstruction> Execute(StageContext context, AttackBaseCommand baseCommand) {
            throw new System.NotImplementedException();
        }
    }
}