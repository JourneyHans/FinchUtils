using System;
using HzFramework.Singleton;

namespace HzFramework.FSM.Stage {
    public class StageManager : ASingleton<StageManager>, ISingleton {

        private StateMachine<StageManager> _fsm;

        public void OnCreate() {
            _fsm = StateMachine<StageManager>.Create(this);
        }

        public void OnUpdate() {
            _fsm.OnLogicUpdate();
        }

        public void OnDestroy() {
            _fsm.Stop();
        }

        public void Register<T>() where T : StageBase {
            _fsm.Register(Activator.CreateInstance<T>());
        }

        public void StartStage<T>() where T : StageBase {
            _fsm.Start<T>();
        }

        public void ChangeStage<T>() where T : StageBase {
            _fsm.ChangeState<T>();
        }

        public bool IsCurrentStage<T>() where T : StageBase {
            return _fsm.CurrentState is T;
        }

        public bool IsCurrentStage<T>(out T stage) where T : StageBase {
            if (_fsm.CurrentState is T currentStage) {
                stage = currentStage;
                return true;
            }

            stage = null;
            return false;
        }

        public void SetData<TData>(TData data) {
            _fsm.SetData<TData>(data);
        }

        public void RemoveData<TData>() {
            _fsm.RemoveData<TData>();
        }

        public TData GetData<TData>(bool disposable = true) {
            TData data = _fsm.GetData<TData>();
            if (disposable) {
                _fsm.RemoveData<TData>();
            }

            return data;
        }
    }
}