using System;
using System.Collections.Generic;

namespace SF.Action {
    public enum EActionType {
        None,
        Attack,
        Move,
    }

    public static class ExtensionCommandType {
        public static EActionType TypeToCommandType(Type type) {
            if (type.BaseType != null) {
                var commandType = type.BaseType.GenericTypeArguments[0];
                var attribute = Attribute.GetCustomAttribute(commandType, typeof(ActionMappingAttribute));
                if (attribute is ActionMappingAttribute commandMapping) {
                    return commandMapping.ActionType;
                }
            }

            return EActionType.None;
        }
    }

    public class CommandEqualityComparer : IEqualityComparer<EActionType> {
        public bool Equals(EActionType x, EActionType y) {
            return x == y;
        }

        public int GetHashCode(EActionType obj) {
            return (int) obj;
        }
    }
}