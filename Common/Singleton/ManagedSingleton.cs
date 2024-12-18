/*
 * 被SingletonSystem管理的单例基类
 * 使用前需要先用SingletonSystem创建并管理其生命周期
 * 确认单例不为空，利用!语法去掉告警，如：Log.Instance!.Error("error message");
 * 否则记得判空，如：Log.Instance?.Error("error message");
 */

#nullable enable
using System;

namespace FinchUtils.Common.Singleton;

public abstract class ManagedSingleton<T> : ISingleton where T : class {
    public static T? Instance { get; private set; }

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

    protected virtual void OnCreate() { }
    protected virtual void OnDestroy() { }
}