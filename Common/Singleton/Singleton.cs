using System;

namespace FinchUtils.Common.Singleton
{
    public abstract class Singleton<T> : ISingleton where T : class {
        public static T Instance { get; private set; }

        public static bool HasCreated => Instance != null;

        void ISingleton.Create() {
            if (HasCreated) {
                throw new Exception($"{typeof(T)} has been created");
            }

            Instance = this as T;
            OnCreate();
        }

        void ISingleton.Destroy() {
            OnDestroy();
            Instance = null;
        }

        protected virtual void OnCreate() {
        }

        protected virtual void OnDestroy() {
        }
    }
}