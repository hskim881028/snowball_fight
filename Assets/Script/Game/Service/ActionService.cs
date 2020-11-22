using System;
using System.Collections.Generic;
using hskim.Action;
using UnityEngine;

namespace hskim {
    public class CommandService {
        private readonly StageContext _context;
        private readonly Queue<BaseAction> _actions = new Queue<BaseAction>();

        private readonly Dictionary<EActionType, ActionHandler> _handlers =
            new Dictionary<EActionType, ActionHandler>(new CommandEqualityComparer());

        private readonly LinkedList<IEnumerator<CustomYieldInstruction>> _runningActions =
            new LinkedList<IEnumerator<CustomYieldInstruction>>();

        public CommandService(StageContext context) {
            _context = context;
            _handlers.Clear();
            var types = GetType().Assembly.GetTypes();
            foreach (var type in types) {
                if (IsValidHandler(type)) {
                    var commandType = ExtensionCommandType.TypeToCommandType(type);
                    if (commandType != EActionType.None) {
                        if (_handlers.ContainsKey(commandType)) {
                            Debug.LogError($"Duplicated command handler : {commandType}");
                            continue;
                        }

                        _handlers.Add(commandType, Activator.CreateInstance(type) as ActionHandler);
                    }
                }
            }
        }

        private bool IsValidHandler(Type type) {
            return type.IsSubclassOf(typeof(ActionHandler)) && type.BaseType != null && type.BaseType.IsGenericType;
        }

        public void Update() {
            ExecuteCommands();
        }

        public void LateUpdate() {
            var node = _runningActions.First;
            while (node != null) {
                var currentNode = node;
                var next = node.Next;
                var value = node.Value;

                if (value.Current == null || value.Current.keepWaiting == false) {
                    try {
                        if (value.MoveNext() == false) {
                            _runningActions.Remove(currentNode);
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
            foreach (var command in _actions) {
                Excute(command);
            }

            _actions.Clear();
        }

        private void Excute(BaseAction baseAction) {
            try {
                if (_handlers.ContainsKey(baseAction.ActionType)) {
                    var handler = _handlers[baseAction.ActionType].Execute(_context, baseAction);
                    if (handler.MoveNext()) {
                        _runningActions.AddLast(handler);
                    }
                }
            }
            catch (Exception e) {
                Debug.LogError(e);
            }
        }

        public void EnqueueCommand(BaseAction action) {
            _actions.Enqueue(action);
        }
    }
}