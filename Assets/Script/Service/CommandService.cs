using System;
using System.Collections.Generic;
using hskim.Command;
using UnityEngine;

namespace hskim {
    public class CommandService {
        readonly Dictionary<ECommandType, CommandHandler> mHandlers = 
            new Dictionary<ECommandType, CommandHandler>(new CommandEqualityComparer());

        readonly Queue<BaseCommand> mCommands = new Queue<BaseCommand>();

        public CommandService() {
            mHandlers.Clear();
            var types = GetType().Assembly.GetTypes();
            foreach (var type in types) {
                if (IsValidHandler(type)) {
                    var commandType = ExtensionCommandType.TypeToCommandType(type);
                    if (commandType != ECommandType.None) {
                        if (mHandlers.ContainsKey(commandType)) {
                            Debug.LogError($"Duplicated command handler : {commandType}");
                            continue;
                        }
                        mHandlers.Add(commandType, Activator.CreateInstance(type) as CommandHandler);
                    }
                }
            }
        }

        bool IsValidHandler(Type type) {
            return type.IsSubclassOf(typeof(CommandHandler)) && 
                   type.BaseType != null && 
                   type.BaseType.IsGenericType;
        }

        public void Update(StageContext context) {
            ExecuteCommands(context);
        }

        void ExecuteCommands(StageContext context) {
            foreach (var command in mCommands) {
                Excute(context, command);
            }
            mCommands.Clear();
        }

        void Excute(StageContext context, BaseCommand baseCommand) {
            try {
                if (mHandlers.ContainsKey(baseCommand.CommandType)) {
                    var handler = mHandlers[baseCommand.CommandType].Execute(context, baseCommand);
                    handler.MoveNext();
                }
            } catch (Exception e) {
                Debug.LogError(e);
            }
        }

        public void EnqueueCommand(BaseCommand command) {
            mCommands.Enqueue(command);
        }
        
    }
}