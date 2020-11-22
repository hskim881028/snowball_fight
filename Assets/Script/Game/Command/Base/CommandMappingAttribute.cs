using System;

namespace hskim.Command {
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandMappingAttribute : Attribute {
        public CommandMappingAttribute(ECommandType commandType) {
            CommandType = commandType;
        }

        public ECommandType CommandType { get; }
    }
}