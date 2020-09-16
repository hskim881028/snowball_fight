using System.Collections.Generic;
using UnityEngine;

namespace hskim.Command {
    public abstract class CommandHandler {
        public abstract IEnumerator<CustomYieldInstruction> Execute(StageContext context, BaseCommand baseCommand);
    }

    public abstract class CommandHandler<T> : CommandHandler where T : BaseCommand {
        public override IEnumerator<CustomYieldInstruction> Execute(StageContext context, BaseCommand baseCommand) {
            return Execute(context, (T) baseCommand);
        }

        protected abstract IEnumerator<CustomYieldInstruction> Execute(StageContext context, T command);
    }
}