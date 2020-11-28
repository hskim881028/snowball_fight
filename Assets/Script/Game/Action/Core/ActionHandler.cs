using System.Collections.Generic;
using UnityEngine;

namespace SF.Action {
    public abstract class ActionHandler {
        public abstract IEnumerator<CustomYieldInstruction> Execute(StageContext context, BaseAction baseAction);
    }

    public abstract class ActionHandler<T> : ActionHandler where T : BaseAction {
        public override IEnumerator<CustomYieldInstruction> Execute(StageContext context, BaseAction baseAction) {
            return Execute(context, (T) baseAction);
        }

        protected abstract IEnumerator<CustomYieldInstruction> Execute(StageContext context, T command);
    }
}