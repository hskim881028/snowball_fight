using System.Collections;

namespace hskim.Command {
    public abstract class CommandHandler {
        public abstract IEnumerator Execute(StageContext context, BaseCommand baseCommand);
    }

    public abstract class CommandHandler<T> : CommandHandler where T : BaseCommand {
        public override IEnumerator Execute(StageContext context, BaseCommand baseCommand) {
            return Execute(context, (T) baseCommand);
        }

        protected abstract IEnumerator Execute(StageContext context, T command);
    }
}