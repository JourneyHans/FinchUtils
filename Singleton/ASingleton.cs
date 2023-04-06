using System;

namespace HzFramework.Singleton {
    public abstract class ASingleton<T> where T : class, ISingleton {
        private static T _instance;

        public static T Instance {
            get {
                if (_instance == null) {
                    throw new Exception($"{typeof(T)} is not created, Use {nameof(SingletonSystem)}.{nameof(SingletonSystem.CreateSingleton)} to create");
                }

                return _instance;
            }
        }

        protected ASingleton() {
            if (_instance != null) {
                throw new Exception($"{typeof(T)} has been created");
            }

            _instance = this as T;
        }

        protected void DestroyInstance() {
            _instance = null;
        }
    }
}