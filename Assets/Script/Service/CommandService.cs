using System;
using System.Collections.Generic;
using hskim.Command;
using UnityEngine;

namespace hskim {
    public class CommandService {
        readonly Dictionary<ECommandType, CommandHandler> mHandlers = 
            new Dictionary<ECommandType, CommandHandler>(new CommandEqualityComparer());

        readonly Queue<BaseCommand> mCommands = new Queue<BaseCommand>();
        readonly LinkedList<IEnumerator<CustomYieldInstruction>> mRunningCommands = new LinkedList<IEnumerator<CustomYieldInstruction>>();

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
        
        public void UpdateRunningCommands()
        {
            var node = mRunningCommands.First;
            while (node != null) {
                var currentNode = node;
                var next = node.Next;
                var value = node.Value;

                if (value.Current == null || value.Current.keepWaiting == false) {
                    try {
                        if (value.MoveNext() == false) {
                            mRunningCommands.Remove(currentNode);
                        }
                    }
                    catch (Exception e) {
                        Debug.LogException(e);
                    }
                }
                node = next;
            }
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
                    if (handler.MoveNext()) {
                        mRunningCommands.AddLast(handler);
                    }
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