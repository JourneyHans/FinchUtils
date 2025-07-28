using System;
using System.Linq;
using FinchUtils.Collections;

namespace FinchUtils.Common.Singleton
{
    public static class SingletonSystem
    {
        private class Wrapper : IComparable<Wrapper>
        {
            public ISingleton Singleton { get; }
            private int Priority { get; }

            public Wrapper(ISingleton singleton, int priority)
            {
                Singleton = singleton;
                Priority = priority;
            }

            public int CompareTo(Wrapper other)
            {
                return Priority.CompareTo(other.Priority);
            }
        }

        private static readonly PriorityQueue<Wrapper> Wrappers = new();

        private static bool Contains<T>() where T : class, ISingleton
        {
            return Wrappers.Any(wrapper => wrapper.Singleton.GetType() == typeof(T));
        }

        public static T CreateSingleton<T>(int priority = 0) where T : class, ISingleton
        {
            if (Contains<T>())
            {
                throw new Exception($"Singleton is already existed: {typeof(T)}");
            }

            T singleton = Activator.CreateInstance<T>();
            Wrapper wrapper = new(singleton, priority);
            singleton.Create();
            Wrappers.Enqueue(wrapper);
            return singleton;
        }

        public static bool DestroySingleton<T>() where T : class, ISingleton
        {
            foreach (Wrapper wrapper in Wrappers)
            {
                if (wrapper.Singleton.GetType() == typeof(T))
                {
                    wrapper.Singleton.Destroy();
                    Wrappers.Remove(wrapper);
                    return true;
                }
            }

            return false;
        }

        public static void DestroyAll()
        {
            foreach (Wrapper wrapper in Wrappers)
            {
                wrapper.Singleton.Destroy();
            }

            Wrappers.Clear();
        }
    }
}