using System;

namespace FinchUtils.Event;

public static class EventDispatcher {
    private static readonly EventInternal EventInternal = new();

    public static void RegEventListener(string eventType, Action handler) {
        EventInternal.RegEventListener(eventType, handler);
    }

    public static void RegEventListener<T>(string eventType, Action<T> handler) {
        EventInternal.RegEventListener(eventType, handler);
    }

    public static void RegEventListener<T1, T2>(string eventType, Action<T1, T2> handler) {
        EventInternal.RegEventListener(eventType, handler);
    }

    public static void RegEventListener<T1, T2, T3>(string eventType, Action<T1, T2, T3> handler) {
        EventInternal.RegEventListener(eventType, handler);
    }

    public static void UnRegEventListener(string eventType, Action handler) {
        EventInternal.UnRegEventListener(eventType, handler);
    }

    public static void UnRegEventListener<T>(string eventType, Action<T> handler) {
        EventInternal.UnRegEventListener(eventType, handler);
    }

    public static void UnRegEventListener<T1, T2>(string eventType, Action<T1, T2> handler) {
        EventInternal.UnRegEventListener(eventType, handler);
    }

    public static void UnRegEventListener<T1, T2, T3>(string eventType, Action<T1, T2, T3> handler) {
        EventInternal.UnRegEventListener(eventType, handler);
    }

    public static void TriggerEvent(string eventType) {
        EventInternal.TriggerEvent(eventType);
    }

    public static void TriggerEvent<T>(string eventType, T arg1) {
        EventInternal.TriggerEvent(eventType, arg1);
    }

    public static void TriggerEvent<T1, T2>(string eventType, T1 arg1, T2 arg2) {
        EventInternal.TriggerEvent(eventType, arg1, arg2);
    }

    public static void TriggerEvent<T1, T2, T3>(string eventType, T1 arg1, T2 arg2, T3 arg3) {
        EventInternal.TriggerEvent(eventType, arg1, arg2, arg3);
    }
}