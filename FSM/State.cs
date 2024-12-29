namespace FinchUtils.FSM;

public abstract class State<T> where T : class {
    public IStateMachine<T> Machine { get; private set; }
    public T Owner { get; private set; }
    public virtual string Name => GetType().Name;
    public bool IsExiting { get; private set; }

    public virtual void OnInit(IStateMachine<T> machine) {
        Machine = machine;
        Owner = machine.Owner;
    }

    public virtual void OnEnter() {
        IsExiting = false;
    }

    public virtual void OnLogicUpdate() {

    }

    public virtual void OnExit() {
        IsExiting = true;
    }

    protected virtual void ChangeState<TState>() where TState : State<T> {
        Machine.ChangeState<TState>();
    }
}