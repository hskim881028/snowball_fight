using System;
using System.Collections.Generic;

namespace hskim.Command {
    public enum ECommandType {
        None,
        Move,
        Attack
    }

    public static class ExtensionCommandType {
        public static ECommandType TypeToCommandType(Type type) {
            if (type.BaseType != null) {
                var commandType = type.BaseType.GenericTypeArguments[0];
                var attribute = Attribute.GetCustomAttribute(commandType, typeof(CommandMappingAttribute));
                if (attribute is CommandMappingAttribute commandMapping) {
                    return commandMapping.CommandType;
                }
            }

            return ECommandType.None;
        }
    }

    public class CommandEqualityComparer : IEqualityComparer<ECommandType> {
        public bool Equals(ECommandType x, ECommandType y) {
            return x == y;
        }

        public int GetHashCode(ECommandType obj) {
            return (int) obj;
        }
    }
}