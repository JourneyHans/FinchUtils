namespace HzFramework.FSM {
    public interface IStateMachine<T> where T : class {
        T Owner { get; }
        void Register(State<T> state);
        void Start<TState>() where TState : State<T>;
        void ChangeState<TState>() where TState : State<T>;
        void Stop(bool clearData = true);

        void SetData<TData>(object data);
        TData GetData<TData>();
        bool RemoveData<TData>();
        void ClearData();
    }
}