using System;
using System.Collections.Generic;

namespace hskim.Command {
    public class BaseCommand {
        static readonly Dictionary<Type, ECommandType> mCachedTypes = new Dictionary<Type, ECommandType>();

        public ECommandType CommandType {
            get {
                if (mCachedTypes.TryGetValue(GetType(), out ECommandType commandType)) {
                    return commandType;
                }

                var attribute = Attribute.GetCustomAttribute(GetType(), typeof(CommandMappingAttribute), false);
                if (attribute is CommandMappingAttribute commandAttribute) {
                    mCachedTypes.Add(GetType(), commandAttribute.CommandType);
                    return commandAttribute.CommandType;
                }
                return ECommandType.None;
            }
        }
    }
}