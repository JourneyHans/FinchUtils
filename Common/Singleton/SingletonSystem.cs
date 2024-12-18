using System;
using FinchUtils.Collections;

namespace FinchUtils.Common.Singleton;

public static class SingletonSystem {
    private class Wrapper : IComparable<Wrapper> {
        public ISingleton Singleton { get; }
        private int Priority { get; }

        public Wrapper(ISingleton singleton, int priority) {
            Singleton = singleton;
            Priority = priority;
        }

        public int CompareTo(Wrapper other) {
            return Priority.CompareTo(other.Priority);
        }
    }

    private static bool _isInitialized;

    private static readonly PriorityQueue<Wrapper> Wrappers = new();

    /// <summary>
    /// 初始化单例系统
    /// </summary>
    public static void Initialize() {
        if (_isInitialized) {
            throw new Exception($"{nameof(SingletonSystem)} is initialized");
        }

        _isInitialized = true;
    }

    /// <summary>
    /// 销毁单例系统
    /// </summary>
    public static void Destroy() {
        if (!_isInitialized) {
            return;
        }

        DestroyAll();
        _isInitialized = false;
    }

    internal static bool Contains<T>() where T : class, ISingleton {
        foreach (Wrapper wrapper in Wrappers) {
            if (wrapper.Singleton.GetType() == typeof(T)) {
                return true;
            }
        }

        return false;
    }

    public static T CreateSingleton<T>(int priority = 0) where T : class, ISingleton {
        if (Contains<T>()) {
            throw new Exception($"Singleton is already existed: {typeof(T)}");
        }

        T singleton = Activator.CreateInstance<T>();
        Wrapper wrapper = new(singleton, priority);
        singleton.Create();
        Wrappers.Enqueue(wrapper);
        return singleton;
    }

    public static bool DestroySingleton<T>() where T : class, ISingleton {
        foreach (Wrapper wrapper in Wrappers) {
            if (wrapper.Singleton.GetType() == typeof(T)) {
                wrapper.Singleton.Destroy();
                Wrappers.Remove(wrapper);
                return true;
            }
        }

        return false;
    }

    private static void DestroyAll() {
        foreach (Wrapper wrapper in Wrappers) {
            wrapper.Singleton.Destroy();
        }

        Wrappers.Clear();
    }
}