using System;

namespace hskim.Action {
    [AttributeUsage(AttributeTargets.Class)]
    public class ActionMappingAttribute : Attribute {
        public ActionMappingAttribute(EActionType actionType) {
            ActionType = actionType;
        }

        public EActionType ActionType { get; }
    }
}