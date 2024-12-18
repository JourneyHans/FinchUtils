using System.Collections.Concurrent;

namespace FinchUtils.ObjectPool;

public static class SimplePool<T> where T : new() {
    private static readonly ConcurrentBag<T> Objects = new();
    private const int MaxPoolSize = 100; // 默认最大池大小

    /// <summary>
    /// 获取当前池中对象的数量。
    /// </summary>
    public static int Count => Objects.Count;

    /// <summary>
    /// 从池中获取一个对象。如果池为空，则创建一个新对象。
    /// </summary>
    /// <returns>池中的对象。</returns>
    public static T Get() {
        // 尝试从池中获取对象
        if (Objects.TryTake(out T item)) {
            return item;
        }

        // 如果池为空，则创建新对象
        return new T();
    }

    /// <summary>
    /// 将对象返回到池中。如果池已满，则丢弃该对象。
    /// </summary>
    public static void Return(T item) {
        // 如果池未满，则添加对象
        if (Objects.Count < MaxPoolSize) {
            Objects.Add(item);
        }
    }

    /// <summary>
    /// 清空池中所有对象。
    /// </summary>
    public static void Clear() {
        Objects.Clear();
    }
}