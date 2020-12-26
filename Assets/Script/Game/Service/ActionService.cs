using System;
using System.Collections.Generic;
using SF.Action;
using UnityEngine;

namespace SF.Service {
    public class ActionService {
        private readonly Queue<BaseAction> _actions = new Queue<BaseAction>();
        private readonly StageContext _context;

        private readonly Dictionary<EActionType, ActionHandler> _handlers =
            new Dictionary<EActionType, ActionHandler>(new ActionEqualityComparer());

        private readonly LinkedList<IEnumerator<CustomYieldInstruction>> _runningActions =
            new LinkedList<IEnumerator<CustomYieldInstruction>>();

        public ActionService(StageContext context) {
            _context = context;
            _handlers.Clear();
            var types = GetType().Assembly.GetTypes();
            foreach (var type in types)
                if (IsValidHandler(type)) {
                    var actionType = ExtensionActionType.TypeToActionType(type);
                    if (actionType != EActionType.None) {
                        if (_handlers.ContainsKey(actionType)) {
                            Debug.LogError($"Duplicated action handler : {actionType}");
                            continue;
                        }

                        _handlers.Add(actionType, Activator.CreateInstance(type) as ActionHandler);
                    }
                }
        }

        private bool IsValidHandler(Type type) {
            return type.IsSubclassOf(typeof(ActionHandler)) && type.BaseType != null && type.BaseType.IsGenericType;
        }

        public void Update() {
            Execute();
        }

        public void LateUpdate() {
            var node = _runningActions.First;
            while (node != null) {
                var currentNode = node;
                var next = node.Next;
                var value = node.Value;

                if (value.Current == null || value.Current.keepWaiting == false)
                    try {
                        if (value.MoveNext() == false) _runningActions.Remove(currentNode);
                    }
                    catch (Exception e) {
                        Debug.LogException(e);
                    }

                node = next;
            }
        }

        private void Execute() {
            foreach (var action in _actions) Excute(action);

            _actions.Clear();
        }

        private void Excute(BaseAction baseAction) {
            try {
                if (_handlers.ContainsKey(baseAction.ActionType)) {
                    var handler = _handlers[baseAction.ActionType].Execute(_context, baseAction);
                    if (handler.MoveNext()) _runningActions.AddLast(handler);
                }
            }
            catch (Exception e) {
                Debug.LogError(e);
            }
        }

        public void EnqueueAction(BaseAction action) {
            _actions.Enqueue(action);
        }
    }
}