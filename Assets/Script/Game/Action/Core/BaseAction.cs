using System;
using System.Collections.Generic;

namespace SF.Action {
    public class BaseAction {
        private static readonly Dictionary<Type, EActionType> CachedTypes = new Dictionary<Type, EActionType>();

        public EActionType ActionType {
            get {
                if (CachedTypes.TryGetValue(GetType(), out var actionType)) {
                    return actionType;
                }

                var attribute = Attribute.GetCustomAttribute(GetType(), typeof(ActionMappingAttribute), false);
                if (attribute is ActionMappingAttribute actionAttribute) {
                    CachedTypes.Add(GetType(), actionAttribute.ActionType);
                    return actionAttribute.ActionType;
                }

                return EActionType.None;
            }
        }
    }
}