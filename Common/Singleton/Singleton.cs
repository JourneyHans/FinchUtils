using System;

namespace HzFramework.Common {
    public abstract class Singleton<T> : ISingleton where T : class {
        private static T _instance;

        public static T Instance {
            get {
                if (_instance == null) {
                    throw new Exception($"{typeof(T)} is not created, Use {nameof(SingletonSystem)}.{nameof(SingletonSystem.CreateSingleton)} to create");
                }

                return _instance;
            }
        }

        void ISingleton.Create() {
            if (_instance != null) {
                throw new Exception($"{typeof(T)} has been created");
            }

            _instance = this as T;
            OnCreate();
        }

        void ISingleton.Destroy() {
            OnDestroy();
            _instance = null;
        }

        protected virtual void OnCreate() { }
        protected virtual void OnDestroy() { }
    }
}