using FinchUtils.Debugger;

namespace FinchUtils.Event {
    internal class EventInternal {
        private readonly Dictionary<string, Delegate> _eventRouter = new();

        private void OnListenerAdding(string eventType, Delegate listenerAdded) {
            _eventRouter.TryAdd(eventType, null);

            Delegate eventDelegate = _eventRouter[eventType];
            if (eventDelegate != null && eventDelegate.GetType() != listenerAdded.GetType()) {
                throw new Exception($"Try to add not correct event {eventType}. Current type is {eventDelegate.GetType().Name}, adding type is {listenerAdded.GetType().Name}");
            }
        }

        private bool OnListenerRemoving(string eventType, Delegate listenerRemoved) {
            if (!_eventRouter.TryGetValue(eventType, out Delegate eventDelegate)) {
                return false;
            }

            if (eventDelegate != null && eventDelegate.GetType() != listenerRemoved.GetType()) {
                throw new Exception($"Remove listener {eventType} failed, Current type is {eventDelegate.GetType()}, adding type is {listenerRemoved.GetType()}.");
            }

            return true;
        }

        private void OnListenerRemoved(string eventType) {
            if (_eventRouter.ContainsKey(eventType) && _eventRouter[eventType] == null) {
                _eventRouter.Remove(eventType);
            }
        }

        public void RegEventListener(string eventType, Action handler) {
            OnListenerAdding(eventType, handler);
            _eventRouter[eventType] = (Action)Delegate.Combine((Action)_eventRouter[eventType], handler);
        }

        public void RegEventListener<T>(string eventType, Action<T> handler) {
            OnListenerAdding(eventType, handler);
            _eventRouter[eventType] = (Action<T>)Delegate.Combine((Action<T>)_eventRouter[eventType], handler);
        }

        public void RegEventListener<T1, T2>(string eventType, Action<T1, T2> handler) {
            OnListenerAdding(eventType, handler);
            _eventRouter[eventType] = (Action<T1, T2>)Delegate.Combine((Action<T1, T2>)_eventRouter[eventType], handler);
        }

        public void RegEventListener<T1, T2, T3>(string eventType, Action<T1, T2, T3> handler) {
            OnListenerAdding(eventType, handler);
            _eventRouter[eventType] = (Action<T1, T2, T3>)Delegate.Combine((Action<T1, T2, T3>)_eventRouter[eventType], handler);
        }

        public void UnRegEventListener(string eventType, Action handler) {
            if (!OnListenerRemoving(eventType, handler)) {
                return;
            }
            _eventRouter[eventType] = (Action)Delegate.Remove((Action)_eventRouter[eventType], handler);
            OnListenerRemoved(eventType);
        }

        public void UnRegEventListener<T>(string eventType, Action<T> handler) {
            if (!OnListenerRemoving(eventType, handler)) {
                return;
            }
            _eventRouter[eventType] = (Action<T>)Delegate.Remove((Action<T>)_eventRouter[eventType], handler);
            OnListenerRemoved(eventType);
        }

        public void UnRegEventListener<T1, T2>(string eventType, Action<T1, T2> handler) {
            if (!OnListenerRemoving(eventType, handler)) {
                return;
            }
            _eventRouter[eventType] = (Action<T1, T2>)Delegate.Remove((Action<T1, T2>)_eventRouter[eventType], handler);
            OnListenerRemoved(eventType);
        }

        public void UnRegEventListener<T1, T2, T3>(string eventType, Action<T1, T2, T3> handler) {
            if (!OnListenerRemoving(eventType, handler)) {
                return;
            }
            _eventRouter[eventType] = (Action<T1, T2, T3>)Delegate.Remove((Action<T1, T2, T3>)_eventRouter[eventType], handler);
            OnListenerRemoved(eventType);
        }

        public void TriggerEvent(string eventType) {
            if (!_eventRouter.TryGetValue(eventType, out var @delegate)) {
                return;
            }

            Delegate[] invocationList = @delegate.GetInvocationList();
            foreach (var invocation in invocationList) {
                if (invocation is not Action action) {
                    throw new Exception($"TriggerEvent {eventType} error: types of parameters are not match.");
                }

                try {
                    action();
                }
                catch (Exception ex)
                {
                    Log.Instance?.Error(ex.ToString());
                }
            }
        }

        public void TriggerEvent<T>(string eventType, T arg1) {
            if (!_eventRouter.TryGetValue(eventType, out var @delegate)) {
                return;
            }
            Delegate[] invocationList = @delegate.GetInvocationList();
            foreach (var invocation in invocationList) {
                if (invocation is not Action<T> action) {
                    throw new Exception($"TriggerEvent {eventType} error: types of parameters are not match.");
                }

                try {
                    action(arg1);
                }
                catch (Exception ex) {
                    Log.Instance?.Error(ex.ToString());
                }
            }
        }

        public void TriggerEvent<T1, T2>(string eventType, T1 arg1, T2 arg2) {
            if (!_eventRouter.TryGetValue(eventType, out var @delegate)) {
                return;
            }
            Delegate[] invocationList = @delegate.GetInvocationList();
            foreach (var invocation in invocationList) {
                if (invocation is not Action<T1, T2> action) {
                    throw new Exception($"TriggerEvent {eventType} error: types of parameters are not match.");
                }

                try {
                    action(arg1, arg2);
                }
                catch (Exception ex) {
                    Log.Instance?.Error(ex.ToString());
                }
            }
        }

        public void TriggerEvent<T1, T2, T3>(string eventType, T1 arg1, T2 arg2, T3 arg3) {
            if (!_eventRouter.TryGetValue(eventType, out var @delegate)) {
                return;
            }
            Delegate[] invocationList = @delegate.GetInvocationList();
            foreach (var invocation in invocationList) {
                if (invocation is not Action<T1, T2, T3> action) {
                    throw new Exception($"TriggerEvent {eventType} error: types of parameters are not match.");
                }

                try {
                    action(arg1, arg2, arg3);
                }
                catch (Exception ex) {
                    Log.Instance?.Error(ex.ToString());
                }
            }
        }
    }
}