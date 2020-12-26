using System;
using System.Collections.Generic;

namespace SF.Action {
    public enum EActionType {
        None,
        Attack,
        Move
    }

    public static class ExtensionActionType {
        public static EActionType TypeToActionType(Type type) {
            if (type.BaseType != null) {
                var actionType = type.BaseType.GenericTypeArguments[0];
                var attribute = Attribute.GetCustomAttribute(actionType, typeof(ActionMappingAttribute));
                if (attribute is ActionMappingAttribute actionMapping) return actionMapping.ActionType;
            }

            return EActionType.None;
        }
    }

    public class ActionEqualityComparer : IEqualityComparer<EActionType> {
        public bool Equals(EActionType x, EActionType y) {
            return x == y;
        }

        public int GetHashCode(EActionType obj) {
            return (int) obj;
        }
    }
}