using System;

namespace hskim.Command {
    [AttributeUsage(AttributeTargets.Class)]
    public class  CommandMappingAttribute : Attribute {
        public ECommandType CommandType { get; }

        public CommandMappingAttribute(ECommandType commandType) {
            CommandType = commandType;
        }
    }
}