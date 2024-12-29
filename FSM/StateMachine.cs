using System;
using System.Collections.Generic;

namespace FinchUtils.FSM;

public class StateMachine<T> : IStateMachine<T> where T : class {
    public T Owner { get; }

    private readonly Dictionary<Type, State<T>> _states;
    private Dictionary<string, object> _keyToData;

    public State<T> PreState { get; private set; }
    public State<T> CurrentState { get; private set; }

    public bool IsStarted => CurrentState != null;

    public StateMachine(T owner) {
        Owner = owner;
        _states = new Dictionary<Type, State<T>>();
    }

    public void Register(State<T> state) {
        if (_states.ContainsKey(state.GetType())) {
            throw new Exception($"The state already exists: {typeof(T)}");
        }

        _states[state.GetType()] = state;
        state.OnInit(this);
    }

    public void Start<TState>() where TState : State<T> {
        if (CurrentState != null) {
            throw new Exception($"StateMachine has been started: {CurrentState.GetType()}");
        }

        if (!_states.TryGetValue(typeof(TState), out var state)) {
            throw new Exception($"Cannot find state: {typeof(TState)}");
        }

        CurrentState = state;
        CurrentState.OnEnter();
    }

    public void ChangeState<TState>() where TState : State<T> {
        if (CurrentState == null) {
            throw new Exception("StateMachine did not start");
        }

        if (!_states.TryGetValue(typeof(TState), out var state)) {
            throw new Exception($"Cannot find state: {typeof(TState)}");
        }

        CurrentState.OnExit();
        PreState = CurrentState;
        CurrentState = state;
        CurrentState.OnEnter();
    }

    public void Stop(bool clearData = true) {
        if (CurrentState == null) {
            throw new Exception("StateMachine has been stopped");
        }

        CurrentState.OnExit();
        PreState = CurrentState;
        CurrentState = null;
        if (clearData) {
            ClearData();
        }
    }

    public void SetData<TData>(object data) {
        if (data == null) {
            throw new Exception("data is null");
        }

        _keyToData ??= new Dictionary<string, object>();
        string key = typeof(TData).FullName;
        if (string.IsNullOrEmpty(key)) {
            throw new Exception($"invalid key for type: {typeof(TData)}");
        }

        _keyToData[key] = data;
    }

    public TData GetData<TData>() {
        string key = typeof(TData).FullName;

        if (string.IsNullOrEmpty(key)) {
            throw new Exception($"invalid key for type: {typeof(TData)}");
        }

        if (_keyToData == null) {
            throw new Exception("no data available.");
        }

        if (_keyToData.TryGetValue(key, out object data)) {
            return (TData)data;
        }

        throw new Exception($"no data for key: {key}");
    }

    public bool RemoveData<TData>() {
        string key = typeof(TData).FullName;
        if (string.IsNullOrEmpty(key)) {
            throw new Exception($"invalid key for type: {typeof(TData)}");
        }

        return _keyToData != null && _keyToData.Remove(key);
    }

    public void ClearData() {
        _keyToData?.Clear();
    }

    public void OnLogicUpdate() {
        CurrentState?.OnLogicUpdate();
    }
}