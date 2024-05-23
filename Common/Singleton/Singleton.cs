using System;

namespace HzFramework.Common {
    public abstract class Singleton<T> : ISingleton where T : class {
        private static T _instance;
        public static T Instance {
            get {
                if (_instance == null) {
                    throw new Exception($"{typeof(T)} not found, need created by SingletonSystem manually");
                }

                return _instance;
            }
        }

        public static bool HasCreated => _instance != null;

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