using System;
using System.Collections.Generic;

namespace hskim.Command {
    public class BaseCommand {
        private static readonly Dictionary<Type, ECommandType> CachedTypes = new Dictionary<Type, ECommandType>();

        public ECommandType CommandType {
            get {
                if (CachedTypes.TryGetValue(GetType(), out var commandType)) {
                    return commandType;
                }

                var attribute = Attribute.GetCustomAttribute(GetType(), typeof(CommandMappingAttribute), false);
                if (attribute is CommandMappingAttribute commandAttribute) {
                    CachedTypes.Add(GetType(), commandAttribute.CommandType);
                    return commandAttribute.CommandType;
                }

                return ECommandType.None;
            }
        }
    }
}