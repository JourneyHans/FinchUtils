using System;

namespace HzFramework.Common {
    public abstract class Singleton<T> : ISingleton where T : class {
        public static T Instance { get; private set; }

        void ISingleton.Create() {
            if (Instance != null) {
                throw new Exception($"{typeof(T)} has been created");
            }

            Instance = this as T;
            OnCreate();
        }

        void ISingleton.Destroy() {
            OnDestroy();
            Instance = null;
        }

        protected virtual void OnCreate() { }
        protected virtual void OnDestroy() { }
    }
}