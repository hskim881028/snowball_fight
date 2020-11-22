using System;
using System.Collections.Generic;
using hskim.Command;
using UnityEngine;

namespace hskim {
    public class CommandService {
        private readonly StageContext _context;
        private readonly Queue<BaseCommand> _commands = new Queue<BaseCommand>();

        private readonly Dictionary<ECommandType, CommandHandler> _handlers =
            new Dictionary<ECommandType, CommandHandler>(new CommandEqualityComparer());

        private readonly LinkedList<IEnumerator<CustomYieldInstruction>> _runningCommands =
            new LinkedList<IEnumerator<CustomYieldInstruction>>();

        public CommandService(StageContext context) {
            _context = context;
            _handlers.Clear();
            var types = GetType().Assembly.GetTypes();
            foreach (var type in types) {
                if (IsValidHandler(type)) {
                    var commandType = ExtensionCommandType.TypeToCommandType(type);
                    if (commandType != ECommandType.None) {
                        if (_handlers.ContainsKey(commandType)) {
                            Debug.LogError($"Duplicated command handler : {commandType}");
                            continue;
                        }

                        _handlers.Add(commandType, Activator.CreateInstance(type) as CommandHandler);
                    }
                }
            }
        }

        private bool IsValidHandler(Type type) {
            return type.IsSubclassOf(typeof(CommandHandler)) && type.BaseType != null && type.BaseType.IsGenericType;
        }

        public void Update() {
            ExecuteCommands();
        }

        public void LateUpdate() {
            var node = _runningCommands.First;
            while (node != null) {
                var currentNode = node;
                var next = node.Next;
                var value = node.Value;

                if (value.Current == null || value.Current.keepWaiting == false) {
                    try {
                        if (value.MoveNext() == false) {
                            _runningCommands.Remove(currentNode);
                        }
                    }
                    catch (Exception e) {
                        Debug.LogException(e);
                    }
                }

                node = next;
            }
        }

        private void ExecuteCommands() {
            foreach (var command in _commands) {
                Excute(command);
            }

            _commands.Clear();
        }

        private void Excute(BaseCommand baseCommand) {
            try {
                if (_handlers.ContainsKey(baseCommand.CommandType)) {
                    var handler = _handlers[baseCommand.CommandType].Execute(_context, baseCommand);
                    if (handler.MoveNext()) {
                        _runningCommands.AddLast(handler);
                    }
                }
            }
            catch (Exception e) {
                Debug.LogError(e);
            }
        }

        public void EnqueueCommand(BaseCommand command) {
            _commands.Enqueue(command);
        }
    }
}