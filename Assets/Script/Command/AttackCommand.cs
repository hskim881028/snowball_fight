using System.Collections;

namespace hskim.Command {
    [CommandMapping(ECommandType.Attack)]
    public class AttackBaseCommand : BaseCommand {
    }

    public class AttackCommandHandler : CommandHandler<AttackBaseCommand> {
        protected override IEnumerator Execute(StageContext context, AttackBaseCommand baseCommand) {
            throw new System.NotImplementedException();
        }
    }
}