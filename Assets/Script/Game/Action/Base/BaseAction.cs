using System;
using System.Collections.Generic;

namespace hskim.Action {
    public class BaseAction {
        private static readonly Dictionary<Type, EActionType> CachedTypes = new Dictionary<Type, EActionType>();

        public EActionType ActionType {
            get {
                if (CachedTypes.TryGetValue(GetType(), out var commandType)) {
                    return commandType;
                }

                var attribute = Attribute.GetCustomAttribute(GetType(), typeof(ActionMappingAttribute), false);
                if (attribute is ActionMappingAttribute commandAttribute) {
                    CachedTypes.Add(GetType(), commandAttribute.ActionType);
                    return commandAttribute.ActionType;
                }

                return EActionType.None;
            }
        }
    }
}